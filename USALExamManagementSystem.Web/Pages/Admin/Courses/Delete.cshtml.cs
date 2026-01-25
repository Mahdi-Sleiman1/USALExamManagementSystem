using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin.Courses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly USALDbContext _context;

        public DeleteModel(USALDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _context.Courses.FindAsync(id);
            if (Course == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var course = await _context.Courses.FindAsync(Course.CourseId);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
