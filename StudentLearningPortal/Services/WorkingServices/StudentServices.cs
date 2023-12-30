using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore;
using StudentLearningPortal.DbModels;
using StudentLearningPortal.Models;
using StudentLearningPortal.Services.Mongo;
using System;
using System.Numerics;
using System.Text.Json;

namespace StudentLearningPortal.Services.WorkingServices
{
    public class StudentService : IStudentService
    {
        private readonly IMongoCollection<Student> _StudentCollection;
        private readonly IMongoCollection<Lecturer> _LecturerCollection;
        private readonly IMongoCollection<Courses> _CourseCollection;
        private readonly IMongoCollection<RegisteredCourses> _RegCoursesCollection;
        private readonly IMongoCollection<NotificationBoard> _NotificationCollection;
        private readonly IMongoCollection<CourseScores> _CourseScoreCollection;
        private readonly IMongoCollection<CourseMaterials> _CourseMaterialsCollection;
        private readonly IConfiguration _config;
        public StudentService(IConfiguration config)
        {
            _config = config;
            string OnlineConnectionString = Sealer.AESEncryption.Decrypt(_config.GetSection("OnlineDb").Value);
            string OnlineDatabaseName = Sealer.AESEncryption.Decrypt(_config.GetSection("DatabaseName").Value);
            string UsersCollection = "Users";
            string CoursesCollection = "Courses";
            string RegCoursesCollection = "RegisteredCourses";
            string NotificationCollection = "NotificationBoard";
            string CourseScoresCollection = "CourseScores";
            string CourseMaterialsCollection = "CourseMaterials";

            var mongoDbHelper = new MongoDbHelper(OnlineConnectionString, OnlineDatabaseName);
            _StudentCollection = mongoDbHelper.GetCollection<Student>(UsersCollection);
            _LecturerCollection = mongoDbHelper.GetCollection<Lecturer>(UsersCollection);
            _CourseCollection = mongoDbHelper.GetCollection<Courses>(CoursesCollection);
            _RegCoursesCollection = mongoDbHelper.GetCollection<RegisteredCourses>(RegCoursesCollection);
            _NotificationCollection = mongoDbHelper.GetCollection<NotificationBoard>(NotificationCollection);
            _CourseScoreCollection = mongoDbHelper.GetCollection<CourseScores>(CourseScoresCollection);
            _CourseMaterialsCollection = mongoDbHelper.GetCollection<CourseMaterials>(CourseMaterialsCollection);
            //_config = config;
        }

        public async Task<string> CreateStudent(Student planet)
        {
            try
            {
                var filter = Builders<Student>.Filter.Eq(x => x.RegistrationNumber, planet.RegistrationNumber);

                var cursor = await _StudentCollection.FindAsync(filter);

                var value = await cursor.FirstOrDefaultAsync();

                if (value != null)
                {
                    return "Registration number already exists";
                }
                //var Value = await _StudentCollection.FindAsync(x => x.RegistrationNumber == planet.RegistrationNumber);
                //if (Value != null)
                //{
                //    return "Registration number already exists";
                //};

                string Password = Sealer.AESEncryption.HashString(planet.Password);
                planet.Password = Password;
                planet.Id = Guid.NewGuid().ToString();

                _StudentCollection.InsertOne(planet);
                return "Added Successfully";
            }catch(Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> CreateLecturer(Lecturer planet)
        {
            try
            {
                var filter = Builders<Lecturer>.Filter.Eq(x => x.StaffNumber , planet.StaffNumber);

                var cursor = await _LecturerCollection.FindAsync(filter);

                var Value = await cursor.FirstOrDefaultAsync();
                //var Value = await _LecturerCollection.FindAsync(x => x.StaffNumber == planet.StaffNumber);
                if (Value != null)
                {
                    return "Staff number already exists";
                };
                string Password = Sealer.AESEncryption.HashString(planet.Password);
                planet.Password = Password;
                planet.Id = Guid.NewGuid().ToString();
                _LecturerCollection.InsertOne(planet);
                return "Added Successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> SignIn(SignIn planet)
        {
            try
            {
                var Lecturerfilter = Builders<Lecturer>.Filter.Eq(x => x.StaffNumber, planet.Id) & Builders<Lecturer>.Filter.Eq(x =>x.Password , Sealer.AESEncryption.HashString(planet.Password));
                var Studentfilter = Builders<Student>.Filter.Eq(x => x.RegistrationNumber, planet.Id) & Builders<Student>.Filter.Eq(x => x.Password, Sealer.AESEncryption.HashString(planet.Password));

                var Lectcursor = await _LecturerCollection.FindAsync(Lecturerfilter);
                var Studentcursor = await _StudentCollection.FindAsync(Studentfilter);

                var LectValue = await Lectcursor.FirstOrDefaultAsync();
                var StudValue = await Studentcursor.FirstOrDefaultAsync();
                if (LectValue == null && StudValue == null)
                {
                    return "Invalid log in credentials";
                };
                
                return "Logged In Successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> ChangePassword(ChangePassword chng)
        {
            try
            {
                var Lecturerfilter = Builders<Lecturer>.Filter.Eq(x => x.StaffNumber, chng.Id) & Builders<Lecturer>.Filter.Eq(x => x.Password, Sealer.AESEncryption.HashString(chng.OldPassword));

                var Studentfilter = Builders<Student>.Filter.Eq(x => x.RegistrationNumber, chng.Id) & Builders<Student>.Filter.Eq(x => x.Password, Sealer.AESEncryption.HashString(chng.OldPassword));

                var Lectcursor = await _LecturerCollection.FindAsync(Lecturerfilter);
                var Studentcursor = await _StudentCollection.FindAsync(Studentfilter);

                Lecturer Value = await Lectcursor.FirstOrDefaultAsync();
                Student Value1 = await Studentcursor.FirstOrDefaultAsync();
                string Pass = Sealer.AESEncryption.HashString(chng.NewPassword);
                if (Value != null)
                {

                    var updateDefinition = Builders<Lecturer>.Update.Set(x => x.Password, Pass);
                    var updateResult = await _LecturerCollection.UpdateOneAsync(Lecturerfilter, updateDefinition);

                    if (updateResult.ModifiedCount > 0)
                    {
                        return "Password changed successfully for Lecturer";
                    }
                    else
                    {
                        return "Failed to update password for Lecturer";
                    }


;
                };

                if (Value1 != null)
                {
                    var updateDefinition = Builders<Student>.Update.Set(x => x.Password, Pass);
                    var updateResult = await _StudentCollection.UpdateOneAsync(Studentfilter, updateDefinition);

                    if (updateResult.ModifiedCount > 0)
                    {
                        return "Password changed successfully for Student";
                    }
                    else
                    {
                        return "Failed to update password for Student";
                    }

                    
                };

                return "User not found";
            }
            catch (Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> EnrollCourse(Courses crs)
        {
            try
            {
                var Lecturerfilter = Builders<Courses>.Filter.Eq(x => x.CourseCode, crs.CourseCode) & Builders<Courses>.Filter.Eq(x => x.ForDepartment, crs.ForDepartment);

                
                var Ccursor = await _CourseCollection.FindAsync(Lecturerfilter);

                var Value = await Ccursor.FirstOrDefaultAsync();
               // var Value1 = await Studentcursor.FirstOrDefaultAsync();
               // var Value = await _CourseCollection.FindAsync(x => x.CourseCode == crs.CourseCode && x.ForDepartment == crs.ForDepartment);
                if (Value != null)
                {
                    return "Course  already exists";
                };

                crs.Id = Guid.NewGuid().ToString();
                _CourseCollection.InsertOne(crs);
                return "Added Successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> EnrollCourses(List<Courses> crs1)
        {

            try
            {
                foreach (Courses crs in crs1)
                {
                    var Lecturerfilter = Builders<Courses>.Filter.Eq(x => x.CourseCode, crs.CourseCode) & Builders<Courses>.Filter.Eq(x => x.ForDepartment, crs.ForDepartment);


                    var Ccursor = await _CourseCollection.FindAsync(Lecturerfilter);

                    var Value = await Ccursor.FirstOrDefaultAsync();
                    if (Value != null)
                    {


                    }
                    else
                    {
                        crs.Id = Guid.NewGuid().ToString();
                        _CourseCollection.InsertOne(crs);
                    }
                }


                    
                
                return "Added Successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong please try again";
            }
        }

        public async Task<string> CourseRegistration(List<RegisteredCourses> planets)
        {
           foreach(RegisteredCourses planet in planets)
            {
                try
                {

                    var Lecturerfilter = Builders<RegisteredCourses>.Filter.Eq(x => x.CourseCode, planet.CourseCode) & Builders<RegisteredCourses>.Filter.Eq(x => x.StudentId, planet.StudentId);


                    var Ccursor = await _RegCoursesCollection.FindAsync(Lecturerfilter);

                    var Value = await Ccursor.FirstOrDefaultAsync();

                    // var Value = await _RegCoursesCollection.FindAsync(x => x.StudentId == planet.StudentId && x.DateRegistered.AddMonths(1) >= planet.DateRegistered );
                    if (Value != null)
                    {
                        //return "Course number already exists";
                    };


                    planet.Id = Guid.NewGuid().ToString();
                    _RegCoursesCollection.InsertOne(planet);
                   
                }
                catch (Exception ex)
                {
                    //return "Something went wrong please try again";
                }
            }
            return "Added Successfully";
        }

        public async Task<string> RemoveCourseRegistration(List<RegisteredCourses> planets)
        {
           foreach(RegisteredCourses planet in planets)
            {
                try
                {
                    var Value = _RegCoursesCollection.FindOneAndDelete(x => x.StudentId == planet.StudentId && x.DateRegistered == planet.DateRegistered);
                    
                }
                catch (Exception ex)
                {
                    //return "Something went wrong please try again";
                }
            }
            return "Removed Successfully";
        }
        public async Task<string> CreateCourseScore(List<CourseScores> planets)
        {
            foreach(CourseScores planet in planets)
            {
                try
                {
                    var Lecturerfilter = Builders<CourseScores>.Filter.Eq(x => x.ForStudentId, planet.ForStudentId) & Builders<CourseScores>.Filter.Eq(x => x.CourseId, planet.CourseId);


                    var Ccursor = await _CourseScoreCollection.FindAsync(Lecturerfilter);

                    var Value = await Ccursor.FirstOrDefaultAsync();

                    if (Value != null)
                    {
                        //return "Score already exists";
                    };
                    planet.Id = Guid.NewGuid().ToString();
                    _CourseScoreCollection.InsertOne(planet);
                    
                }
                catch (Exception ex)
                {
                    //return "Something went wrong please try again";
                }
            }
            return "Added Successfully";

        }

        public async Task<string> SendNotification(List<NotificationBoard> planets)
        {
            foreach(NotificationBoard planet in planets)
            {
                try
                {
                    planet.Id = Guid.NewGuid().ToString();
                    _NotificationCollection.InsertOne(planet);
                    
                }
                catch (Exception ex)
                {
                    //return "Something went wrong please try again";
                }
            }
            return "Added Successfully";
        }


        public async Task<Student> getStudent(string StudentId)
        {
            Student OneStudent = (Student)_StudentCollection.Find(id => id.RegistrationNumber == StudentId);

            return OneStudent;
        }



        public async Task<List<Courses>> GetStudentToRegisterCourse(string StudentCode)
        {
            var Studentfilter = Builders<Student>.Filter.Eq(x => x.RegistrationNumber, StudentCode);

            var Studentcursor = await _StudentCollection.FindAsync(Studentfilter);
            Student StudValue = await Studentcursor.FirstOrDefaultAsync();

            if (StudValue == null)
            {
                return null;
            }


            var Coursesfilter = Builders<Courses>.Filter.Eq(x => x.ForLevel, StudValue.Level);
            //&                    Builders<Courses>.Filter.In(x => x.ForDepartment, new List<string> { StudValue.Department });

            var Coursecursor = await _CourseCollection.FindAsync(Coursesfilter);
            List<Courses> CourseValue = await Coursecursor.ToListAsync();

            CourseValue = CourseValue.Where(crs=>crs.ForDepartment.Contains(StudValue.Department)).ToList();

            return CourseValue;





            //var Coursesfilter = Builders<Courses>.Filter.Eq(x => x.ForLevel, StudValue.Level) &
            //        Builders<Courses>.Filter.ElemMatch(x => x.ForDepartment, department => department == StudValue.Department);

            //var Coursecursor = await _CourseCollection.FindAsync(Coursesfilter);
            //var CourseValue =  Coursecursor.ToList();

            //return CourseValue;

        }

        public async Task<Lecturer> getLecturer(string LecturerId)
        {
            Lecturer OneStudent = (Lecturer)_LecturerCollection.Find(id => id.StaffNumber == LecturerId);

            return OneStudent;
        }

        public async Task<Courses> getCourses(string CourseCode)
        {
            Courses OneStudent = (Courses)_CourseCollection.Find(id => id.CourseCode == CourseCode);

            return OneStudent;
        }

        public async Task<CourseMaterialz> getCourseMaterials(string CourseCode)
        {
            List<CourseMaterials> OneStudent = (List<CourseMaterials>)_CourseMaterialsCollection.Find(id => id.ForCourseId == CourseCode).ToList();

            return null;
        }

        public string UploadCourseMaterials(CourseMaterialz cms)
        {
            List<IFormFile> files = cms.Files;
            if (files == null || files.Count == 0)
            {
                return "Invalid file";
            }

            foreach (IFormFile file in files)
            {
                try
                {

                    string contentType = file.ContentType;

                    if (IsValidFileType(contentType))
                    {
                        // Create a folder based on the content type and timestamp
                        string folderName = $"{contentType.ToString().ToCamelCase}";
                        string folderPath = Path.Combine("Files", folderName);

                        // Create the folder if it doesn't exist
                        Directory.CreateDirectory(folderPath);

                        // Generate a unique file name
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        // Combine the folder path and file name to get the full path
                        string filePath = Path.Combine(folderPath, fileName);

                        // Save the file to the specified path
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        string json = JsonSerializer.Serialize(file);

                        // Deserialize the JSON string into a new instance of a similar class
                        CourseMaterials destinationPerson = JsonSerializer.Deserialize<CourseMaterials>(json);

                        destinationPerson.FileName = fileName;
                        destinationPerson.FilePath = filePath;

                        _CourseMaterialsCollection.InsertOne(destinationPerson);


                    }
                    
                }
                catch (Exception ex)
                {
                    return  $"Internal server error: {ex.Message}";
                }
            }

            return $"File uploaded successfully files";

        }

        private bool IsValidFileType(string contentType)
        {
            // Define the allowed file types
            string[] allowedTypes = { "video", "pdf", "word", "audio" };

            // Check if the content type contains any of the allowed types
            return allowedTypes.Any(type => contentType.Contains(type, StringComparison.OrdinalIgnoreCase));
        }
           
    }

}

 