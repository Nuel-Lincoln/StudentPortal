using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class Lecturer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StaffNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GradeLevel { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }
    }
}
