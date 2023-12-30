using StudentLearningPortal.DbModels;
using StudentLearningPortal.Models;

namespace StudentLearningPortal.Services.WorkingServices
{
    public interface IStudentService
    {
        public  Task<List<Student>> GetStudents();
        public Task<string> CreateStudent(Student student);

        public string UploadCourseMaterials(CourseMaterialz cms);

        public Task<CourseMaterialz> getCourseMaterials(string CourseCode);

        //public  Task<string> EnrollCourses(List<Courses> crs1);
        public  Task<List<Courses>> GetStudentToRegisterCourse(string StudentCode);
        public Task<Courses> getCourses(string CourseCode);

        public Task<Lecturer> getLecturer(string LecturerId);

        public Task<Student> getStudent(string StudentId);

        public Task<string> SendNotification(List<NotificationBoard> planet);

        public Task<string> CreateCourseScore(List<CourseScores> planet);

        public Task<string> RemoveCourseRegistration(List<RegisteredCourses> planet);

        public Task<string> CourseRegistration(List<RegisteredCourses> planet);

        public Task<string> EnrollCourses(List<Courses> crs1);

        public Task<string> EnrollCourse(Courses crs);

        public Task<string> ChangePassword(ChangePassword chng);

        public Task<string> SignIn(SignIn planet);

        public Task<string> CreateLecturer(Lecturer planet);
    }
}
