using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize(Roles = "Admin,Doctor")]
    public class DeleteModel : PageModel
    {
        private readonly USALDbContext _context;

        public DeleteModel(USALDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Exam Exam { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Exam = await _context.Exams
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (Exam == null)
                return NotFound();

            var email = User.Identity!.Name;
            var user = await _context.UsersProfile.FirstOrDefaultAsync(u => u.Email == email);

            if (User.IsInRole("Doctor") && Exam.Course.MajorId != user!.MajorId)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var exam = await _context.Exams.FindAsync(Exam.ExamId);
            if (exam == null) return NotFound();

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
