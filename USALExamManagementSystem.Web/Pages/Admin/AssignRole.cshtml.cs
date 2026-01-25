using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Models;

namespace USALExamManagementSystem.Web.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AssignRoleModel : PageModel
    {
        private readonly USALDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AssignRoleModel(
            USALDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<IdentityUser> IdentityUsers { get; set; } = [];
        public List<Role> Roles { get; set; } = [];
        public List<Major> Majors { get; set; } = [];

        [BindProperty] public string UserId { get; set; } = "";
        [BindProperty] public int RoleId { get; set; }
        [BindProperty] public int? MajorId { get; set; }

        public async Task OnGetAsync()
        {
            IdentityUsers = _userManager.Users.ToList();
            Roles = await _context.Roles.ToListAsync();
            Majors = await _context.Majors.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var identityUser = await _userManager.FindByIdAsync(UserId);
            if (identityUser == null) return Page();

            // 🔹 Remove old Identity roles
            var currentRoles = await _userManager.GetRolesAsync(identityUser);
            await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

            // 🔹 Assign new Identity role
            string identityRole = RoleId switch
            {
                1 => "Admin",
                2 => "Doctor",
                _ => "Student"
            };

            await _userManager.AddToRoleAsync(identityUser, identityRole);

            // 🔹 Update app profile
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
                    MajorId = RoleId == 2 ? MajorId : null
                };

                _context.UsersProfile.Add(profile);
            }
            else
            {
                profile.RoleId = RoleId;
                profile.MajorId = RoleId == 2 ? MajorId : null;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
