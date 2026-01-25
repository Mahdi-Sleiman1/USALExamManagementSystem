using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly USALDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersModel(USALDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<IdentityUser> IdentityUsers { get; set; } = new();
        public List<Major> Majors { get; set; } = new();

        [BindProperty] public string UserId { get; set; } = "";
        [BindProperty] public int RoleId { get; set; }
        [BindProperty] public int? MajorId { get; set; }

        public async Task OnGetAsync()
        {
            IdentityUsers = _userManager.Users.ToList();
            Majors = await _context.Majors.OrderBy(m => m.MajorName).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUsers = _userManager.Users.ToList();
            Majors = await _context.Majors.ToListAsync();

            var identityUser = await _userManager.FindByIdAsync(UserId);
            if (identityUser == null) return RedirectToPage();

            // Remove old roles
            var roles = await _userManager.GetRolesAsync(identityUser);
            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(identityUser, roles);

            string roleName = RoleId switch
            {
                1 => "Admin",
                2 => "Doctor",
                _ => "Student"
            };

            await _userManager.AddToRoleAsync(identityUser, roleName);

            // Sync User Profile
            var profile = await _context.UsersProfile
                .FirstOrDefaultAsync(u => u.UserId == identityUser.Id);

            if (profile == null)
            {
                profile = new User
                {
                    UserId = identityUser.Id,
                    Email = identityUser.Email!,
                    FullName = identityUser.Email!,
                    RoleId = RoleId,
                    MajorId = RoleId == 1 ? null : MajorId
                };
                _context.UsersProfile.Add(profile);
            }
            else
            {
                profile.RoleId = RoleId;
                profile.MajorId = RoleId == 1 ? null : MajorId;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
