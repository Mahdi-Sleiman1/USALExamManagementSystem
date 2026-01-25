using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace USALExamManagementSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            public string Code { get; set; } = string.Empty;
        }

        public void OnGet(string code = null!)
        {
            Input.Code = code;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
                return RedirectToPage("./ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(
                user,
                Input.Code,
                Input.Password);

            if (result.Succeeded)
                return RedirectToPage("./ResetPasswordConfirmation");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
