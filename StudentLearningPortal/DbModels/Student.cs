using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class Student
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RegistrationNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Level { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }
    }
}
