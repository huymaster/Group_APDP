using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class Course
{
    [Required] [MaxLength(36)] public string CourseId { get; set; } = Guid.NewGuid().ToString();

    [Required] [MaxLength(12)] public required string CourseCode { get; set; }

    [Required] [MaxLength(50)] public required string CourseName { get; set; }

    [MaxLength(100)] public string? Description { get; set; }

    [Required] public required int Credits { get; set; }

    [Required] public required DateOnly StartDate { get; set; }

    [Required] public required DateOnly EndDate { get; set; }

    [Required] public required User Teacher { get; set; }

    [NotMapped] public ICollection<AssignedUser> AssignedUsers { get; set; } = new List<AssignedUser>();

    public long GetCourseDuration()
    {
        return EndDate.DayNumber - StartDate.DayNumber;
    }
}