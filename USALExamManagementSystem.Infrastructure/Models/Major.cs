using System;
using System.Collections.Generic;

namespace USALExamManagementSystem.Infrastructure.Models;

public partial class Major
{
    public int MajorId { get; set; }

    public string MajorName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
