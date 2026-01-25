using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize] // ✅ KEEP AUTHORIZATION
    public class DetailsModel : PageModel
    {
        private readonly USALDbContext _context;

        public DetailsModel(USALDbContext context)
        {
            _context = context;
        }

        public Exam Exam { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var email = User.Identity!.Name;

            var user = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return Forbid();

            // Load exam with course + major
            var examQuery = _context.Exams
                .Include(e => e.Course)
                .ThenInclude(c => c.Major)
                .Where(e => e.ExamId == id);

            // Admin → access all
            if (user.RoleId != 1)
            {
                examQuery = examQuery
                    .Where(e => e.Course.MajorId == user.MajorId);
            }

            Exam = await examQuery.FirstOrDefaultAsync();

            if (Exam == null)
                return NotFound();

            return Page();
        }
    }
}
