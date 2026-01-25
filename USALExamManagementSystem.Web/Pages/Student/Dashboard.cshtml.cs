using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace USALExamManagementSystem.Web.Pages.Student;

[Authorize]
public class DashboardModel : PageModel
{
    public void OnGet() { }
}
