namespace USALExamManagementSystem.Infrastructure.Models;

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public int MajorId { get; set; }

    public Major Major { get; set; } = null!;
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
