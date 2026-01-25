using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using USALExamManagementSystem.Infrastructure.Models;

public class Exam
{
    public int ExamId { get; set; }

    [Required(ErrorMessage = "Exam title is required.")]
    public string ExamTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Exam date is required.")]
    public DateTime ExamDate { get; set; }

    [Required(ErrorMessage = "Course is required.")]
    public int CourseId { get; set; }

    [ValidateNever] // 🔥 THIS IS THE KEY FIX
    public Course Course { get; set; } = null!;

    public string? FilePath { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
