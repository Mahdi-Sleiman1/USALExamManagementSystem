using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace USALExamManagementSystem.Web.Pages.Student
{
    [Authorize(Roles = "Student")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
