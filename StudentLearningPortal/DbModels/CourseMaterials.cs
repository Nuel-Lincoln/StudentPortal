using MongoDB.Bson;

namespace StudentLearningPortal.DbModels
{
    public class CourseMaterials
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ForCourseId { get; set; }

        public string FileName { get; set; }

        public string UploadedBy { get; set; }

        public string FileType { get; set; }
        public DateTime UploadTime { get; set; }

        public string? FilePath { get; set; }

        public bool IsVisible { get; set; }
    }
}
