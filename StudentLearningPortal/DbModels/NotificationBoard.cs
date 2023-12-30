using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class NotificationBoard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ForStdentId { get; set; }

        public string ForCourseId { get; set; }

        public DateTime SentTime { get; set; }

        public bool IsSent { get; set; }

        public bool IsSeen { get; set; }
    }
}
