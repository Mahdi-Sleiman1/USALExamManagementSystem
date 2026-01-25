using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin.Courses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly USALDbContext _context;

        public CreateModel(USALDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = new();

        public List<Major> Majors { get; set; } = [];

        public async Task OnGetAsync()
        {
            Majors = await _context.Majors.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
