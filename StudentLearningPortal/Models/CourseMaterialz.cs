using MongoDB.Bson;

namespace StudentLearningPortal.Models
{
    public class CourseMaterialz
    {       
            public ObjectId Id { get; set; }
            public string ForCourseCode { get; set; }

            public List<IFormFile> Files { get; set; }

            public string UploadedBy { get; set; }

            public DateTime UploadTime { get; set; }

            
            public bool IsVisible { get; set; }
        
    }
}
