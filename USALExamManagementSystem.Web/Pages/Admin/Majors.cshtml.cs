using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class MajorsModel : PageModel
    {
        private readonly USALDbContext _context;

        public MajorsModel(USALDbContext context)
        {
            _context = context;
        }

        public List<Major> Majors { get; set; } = new();

        public async Task OnGetAsync()
        {
            Majors = await _context.Majors
                .OrderBy(m => m.MajorName)
                .ToListAsync();
        }
    }
}
