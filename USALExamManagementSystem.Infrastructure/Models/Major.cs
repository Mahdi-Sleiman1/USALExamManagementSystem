namespace USALExamManagementSystem.Infrastructure.Models;

public class Major
{
    public int MajorId { get; set; }
    public string MajorName { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
