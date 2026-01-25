using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly USALDbContext _context;

        public IndexModel(USALDbContext context)
        {
            _context = context;
        }

        public List<Exam> Exams { get; set; } = new();

        public async Task OnGetAsync()
        {
            var email = User.Identity!.Name;

            var user = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                Exams = [];
                return;
            }

            // 🔴 ADMIN → ALL EXAMS
            if (user.RoleId == 1)
            {
                Exams = await _context.Exams
                    .Include(e => e.Course)
                    .OrderByDescending(e => e.ExamDate)
                    .ToListAsync();
                return;
            }

            // 🔵 STUDENT / DOCTOR → ONLY THEIR MAJOR
            Exams = await _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.MajorId == user.MajorId)
                .OrderByDescending(e => e.ExamDate)
                .ToListAsync();
        }
    }
}
