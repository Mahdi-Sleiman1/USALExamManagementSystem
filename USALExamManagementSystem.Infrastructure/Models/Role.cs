using USALExamManagementSystem.Infrastructure.Models;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    // ✅ ADD THIS
    public ICollection<User> Users { get; set; } = new List<User>();
}
