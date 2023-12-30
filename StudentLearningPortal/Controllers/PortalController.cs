using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentLearningPortal.DbModels;
using StudentLearningPortal.Models;
using StudentLearningPortal.Services.WorkingServices;

namespace StudentLearningPortal.Controllers
{
    
    [ApiController]
    public class PortalController : ControllerBase
    {
        private readonly IStudentService _std;
        public PortalController(IStudentService std)
        {
            _std = std;
        }

        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
            var Std = await _std.GetStudents();

            return Ok(Std);
        }

        [HttpGet("GetStudents1")]
        public async Task<IActionResult> GetStudents1()
        {
            var Std = await _std.GetStudents();

            return Ok(Std);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignIn std)
        {
            string Std = await _std.SignIn(std);

            return Ok(Std);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword std)
        {
            string Std = await _std.ChangePassword(std);

            return Ok(Std);
        }


        [HttpPost("EnrollStudent")]
        public async  Task<IActionResult> CreateStudent(Student std)
        {
            string Std = await _std.CreateStudent(std);

            return Ok(Std);
        }


        [HttpPost("EnrollLecturer")]
        public async Task<IActionResult> CreateLecturer(Lecturer std)
        {
            string Std = await _std.CreateLecturer(std);

            return Ok(Std);
        }

        [HttpPost("EnrollCourse")]
        public async Task<IActionResult> CreateCourse(Courses std)
        {
            string Std = await _std.EnrollCourse(std);

            return Ok(Std);
        }

        [HttpPost("GetStudentToRegisterCourse")]
        public async Task<IActionResult> RegisterCourse(string StudentReg)
        {
            var Std = await _std.GetStudentToRegisterCourse(StudentReg);

            return Ok(Std);
        }

        [HttpPost("RegisterCourse")]
        public async Task<IActionResult> RegisterCourse(List<RegisteredCourses> std)
        {
            string Std = await _std.CourseRegistration(std);

            return Ok(Std);
        }

        [HttpPost("UnregisterCourse")]
        public async Task<IActionResult> UnregisterCourse(List<RegisteredCourses> std)
        {
            string Std = await _std.RemoveCourseRegistration(std);

            return Ok(Std);
        }

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification(List<NotificationBoard> std)
        {
            string Std = await _std.SendNotification(std);

            return Ok(Std);
        }

        [HttpPost("ScoreCourse")]
        public async Task<IActionResult> ScoreCourse(List<CourseScores> std)
        {
            string Std = await _std.CreateCourseScore(std);

            return Ok(Std);
        }

        [HttpPost("UploadCourseMaterial")]
        public  IActionResult UploadCourseMaterial(CourseMaterialz std)
        {
            string Std =  _std.UploadCourseMaterials(std);

            return Ok(Std);
        }
    }
}
