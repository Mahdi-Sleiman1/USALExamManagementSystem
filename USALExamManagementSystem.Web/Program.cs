using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using USALExamManagementSystem.Infrastructure.Identity;
using USALExamManagementSystem.Infrastructure.Models;
using USALExamManagementSystem.Web.Data;
using USALExamManagementSystem.Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// =======================
// DOMAIN DATABASE
// =======================
builder.Services.AddDbContext<USALDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =======================
// IDENTITY DATABASE
// =======================
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =======================
// IDENTITY CONFIG
// =======================
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// =======================
// EMAIL SENDER (FIX)
// =======================
builder.Services.AddTransient<IEmailSender, EmailSender>();

// =======================
// MVC + Razor Pages
// =======================
//builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// =======================
// SEED ROLES
// =======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAsync(services);
}


async Task SeedRolesAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Doctor", "Student" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    await SeedRolesAsync(scope.ServiceProvider);
}


// =======================
// MIDDLEWARE
// =======================
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapRazorPages();
app.MapControllers();

app.Run();
