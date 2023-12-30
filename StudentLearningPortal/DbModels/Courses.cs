using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class Courses
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CourseCode { get; set; }

        public string CourseDescription { get; set; }

        public int ForLevel { get; set; }

        public List<string> ForDepartment { get; set; }

        public string ForSemester { get; set; }
    }
}
