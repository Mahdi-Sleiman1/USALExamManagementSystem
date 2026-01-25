using USALExamManagementSystem.Infrastructure.Models;

public class User
{
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }      // 1=Admin, 2=Doctor, 3=Student
    public int? MajorId { get; set; }    // 🔑 STUDENT MAJOR

    public Role Role { get; set; } = null!;
    public Major? Major { get; set; }
}
