using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize(Roles = "Admin,Doctor")]
    public class EditModel : PageModel
    {
        private readonly USALDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(USALDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Exam Exam { get; set; } = null!;

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public List<Course> Courses { get; set; } = [];

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

            Courses = await _context.Courses
                .Where(c => !User.IsInRole("Doctor") || c.MajorId == user!.MajorId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (UploadFile != null)
            {
                var dir = Path.Combine(_env.WebRootPath, "uploads", "exams");
                Directory.CreateDirectory(dir);

                var name = Guid.NewGuid() + Path.GetExtension(UploadFile.FileName);
                var path = Path.Combine(dir, name);

                using var stream = new FileStream(path, FileMode.Create);
                await UploadFile.CopyToAsync(stream);

                Exam.FilePath = "/uploads/exams/" + name;
            }

            _context.Exams.Update(Exam);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
