using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly USALDbContext _context;

        public DashboardModel(USALDbContext context)
        {
            _context = context;
        }

        public int Doctors { get; set; }
        public int Students { get; set; }
        public int Majors { get; set; }
        public int Courses { get; set; }
        public int Exams { get; set; }

        public async Task OnGetAsync()
        {
            // Doctor = RoleId == 2
            Doctors = await _context.UsersProfile
                .CountAsync(u => u.RoleId == 2);

            // Student = NOT Admin (1) AND NOT Doctor (2)
            Students = await _context.UsersProfile
                .CountAsync(u => u.RoleId != 1 && u.RoleId != 2);

            Majors = await _context.Majors.CountAsync();
            Courses = await _context.Courses.CountAsync();
            Exams = await _context.Exams.CountAsync();
        }
    }
}
