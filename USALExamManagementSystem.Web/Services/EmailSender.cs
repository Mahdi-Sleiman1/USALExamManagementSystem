using Microsoft.AspNetCore.Identity.UI.Services;

namespace USALExamManagementSystem.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Development only – no real email
            return Task.CompletedTask;
        }
    }
}
