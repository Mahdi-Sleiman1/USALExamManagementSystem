using System;
using System.Collections.Generic;

namespace USALExamManagementSystem.Infrastructure.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int MajorId { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual Major Major { get; set; } = null!;
}
