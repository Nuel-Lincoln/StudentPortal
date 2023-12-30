using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class CourseScores
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CourseId { get; set; }

        public string ForStudentId { get; set; }

        public string ScoreFor { get; set; }

        public int TotalScore { get; set; }

        public int ActualScore { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
