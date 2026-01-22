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

        public IList<Exam> Exams { get; set; } = new List<Exam>();

        public async Task OnGetAsync()
        {
            Exams = await _context.Exams
                .Include(e => e.Course)
                .ToListAsync();
        }
    }
}
