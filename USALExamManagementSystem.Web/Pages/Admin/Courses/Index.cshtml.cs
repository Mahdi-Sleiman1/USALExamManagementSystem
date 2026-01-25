using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin.Courses
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly USALDbContext _context;

        public IndexModel(USALDbContext context)
        {
            _context = context;
        }

        public List<Course> Courses { get; set; } = [];

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Major)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }
    }
}
