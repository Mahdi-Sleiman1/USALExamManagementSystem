using System;
using System.Collections.Generic;

namespace USALExamManagementSystem.Infrastructure.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public string ExamTitle { get; set; } = null!;

    public DateOnly ExamDate { get; set; }

    public int CourseId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
