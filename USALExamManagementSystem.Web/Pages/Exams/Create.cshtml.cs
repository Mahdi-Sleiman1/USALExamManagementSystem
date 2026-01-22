using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Exams
{
    [Authorize(Roles = "Doctor")]
    public class CreateModel : PageModel
    {
        private readonly USALDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(USALDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Exam Exam { get; set; }

        public List<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Courses = await _context.Courses.ToListAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Exam.CreatedBy = user.Id;

            _context.Exams.Add(Exam);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
