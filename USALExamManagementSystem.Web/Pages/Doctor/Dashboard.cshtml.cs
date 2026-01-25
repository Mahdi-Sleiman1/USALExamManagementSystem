using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Doctor
{
    public class DashboardModel : PageModel
    {
        private readonly USALDbContext _context;

        public DashboardModel(USALDbContext context)
        {
            _context = context;
        }

        public List<Course> Courses { get; set; } = new();
        public List<Exam> Exams { get; set; } = new();

        public async Task OnGetAsync()
        {
            var email = User.Identity!.Name;

            var doctor = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email && u.RoleId == 2);

            if (doctor == null || doctor.MajorId == null)
                return;

            Courses = await _context.Courses
                .Where(c => c.MajorId == doctor.MajorId)
                .ToListAsync();

            Exams = await _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.MajorId == doctor.MajorId)
                .OrderByDescending(e => e.ExamDate)
                .ToListAsync();
        }
    }
}
