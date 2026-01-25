using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize(Roles = "Doctor,Admin")]
    public class CreateModel : PageModel
    {
        private readonly USALDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(USALDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Exam Exam { get; set; } = new();

        [BindProperty]
        public IFormFile? UploadFile { get; set; }

        public List<Course> Courses { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadCoursesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCoursesAsync();

            if (Exam.CourseId == 0)
            {
                ModelState.AddModelError("Exam.CourseId", "Please select a course.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ================= FILE UPLOAD =================
            if (UploadFile != null && UploadFile.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "exams");
                Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(UploadFile.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await UploadFile.CopyToAsync(stream);

                Exam.FilePath = "/uploads/exams/" + fileName;
            }

            Exam.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            _context.Exams.Add(Exam);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadCoursesAsync()
        {
            // =========================
            // 🔹 ADMIN → ALL COURSES
            // =========================
            if (User.IsInRole("Admin"))
            {
                Courses = await _context.Courses
                    .OrderBy(c => c.CourseName)
                    .ToListAsync();
                return;
            }

            // =========================
            // 🔹 DOCTOR → HIS MAJOR
            // =========================
            var email = User.Identity!.Name;

            var user = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.MajorId == null)
            {
                Courses = new();
                return;
            }

            Courses = await _context.Courses
                .Where(c => c.MajorId == user.MajorId)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }
    }
    }
