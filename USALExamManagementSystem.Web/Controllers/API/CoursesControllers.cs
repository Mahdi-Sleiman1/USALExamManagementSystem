using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Controllers.Api
{
    [Route("api/courses")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly USALDbContext _context;

        public CoursesController(USALDbContext context)
        {
            _context = context;
        }

        // 🔹 ADMIN → ALL COURSES
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses
                .Include(c => c.Major)
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName,
                    Major = c.Major.MajorName
                })
                .ToListAsync();

            return Ok(courses);
        }

        // 🔹 DOCTOR → COURSES OF HIS MAJOR
        [HttpGet("my")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyCourses()
        {
            var email = User.Identity!.Name;

            var doctor = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email && u.RoleId == 2);

            if (doctor == null || doctor.MajorId == null)
                return Ok(new List<object>());

            var courses = await _context.Courses
                .Where(c => c.MajorId == doctor.MajorId)
                .Select(c => new
                {
                    c.CourseId,
                    c.CourseName
                })
                .ToListAsync();

            return Ok(courses);
        }
    }
}
