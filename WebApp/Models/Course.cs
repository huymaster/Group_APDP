using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class Course
{
    [Display(Name = "Course ID")]
    [Required]
    [MaxLength(36)]
    public string CourseId { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Course code")]
    [Required]
    [MaxLength(12)]
    public required string CourseCode { get; set; }

    [Display(Name = "Course name")]
    [Required]
    [MaxLength(50)]
    public required string CourseName { get; set; }

    [Display(Name = "Course description")]
    [MaxLength(100)]
    public string? Description { get; set; }

    [Display(Name = "Credits")] [Required] public required int Credits { get; set; }

    [Display(Name = "Start date")]
    [Required]
    public required DateOnly StartDate { get; set; }

    [Display(Name = "End date")]
    [Required]
    public required DateOnly EndDate { get; set; }

    [Display(Name = "Teacher ID")] public string? TeacherId { get; set; }

    [ForeignKey("TeacherId")] public User? Teacher { get; set; } = null;

    public ICollection<AssignedUser> AssignedUsers { get; set; } = new List<AssignedUser>();

    public long GetCourseDuration()
    {
        return EndDate.DayNumber - StartDate.DayNumber;
    }
}