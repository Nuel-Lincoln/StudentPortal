using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class RegisteredCourses
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentId { get; set; }

        public string CourseId { get; set; }

        public string CourseCode { get; set; }

        public DateTime DateRegistered { get; set; }

        public string Lecturer { get; set; }
    }
}
