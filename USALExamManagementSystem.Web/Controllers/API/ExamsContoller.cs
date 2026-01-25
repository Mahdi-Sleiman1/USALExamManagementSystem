using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Controllers.Api
{
    [Route("api/exams")]
    [ApiController]
    [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly USALDbContext _context;

        public ExamsController(USALDbContext context)
        {
            _context = context;
        }

        // 🔹 ADMIN → ALL EXAMS
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _context.Exams
                .Include(e => e.Course)
                .ThenInclude(c => c.Major)
                .OrderByDescending(e => e.ExamDate)
                .Select(e => new
                {
                    e.ExamId,
                    e.ExamTitle,
                    e.ExamDate,
                    Course = e.Course.CourseName,
                    Major = e.Course.Major.MajorName
                })
                .ToListAsync();

            return Ok(exams);
        }

        // 🔹 STUDENT / DOCTOR → EXAMS OF THEIR MAJOR
        [HttpGet("my")]
        public async Task<IActionResult> GetMyExams()
        {
            var email = User.Identity!.Name;

            var user = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.MajorId == null)
                return Ok(new List<object>());

            var exams = await _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.MajorId == user.MajorId)
                .OrderByDescending(e => e.ExamDate)
                .Select(e => new
                {
                    e.ExamId,
                    e.ExamTitle,
                    e.ExamDate,
                    Course = e.Course.CourseName
                })
                .ToListAsync();

            return Ok(exams);
        }

        // 🔹 EXAM DETAILS
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Course)
                .ThenInclude(c => c.Major)
                .Where(e => e.ExamId == id)
                .Select(e => new
                {
                    e.ExamId,
                    e.ExamTitle,
                    e.ExamDate,
                    Course = e.Course.CourseName,
                    Major = e.Course.Major.MajorName,
                    e.FilePath
                })
                .FirstOrDefaultAsync();

            if (exam == null)
                return NotFound();

            return Ok(exam);
        }
    }
}
