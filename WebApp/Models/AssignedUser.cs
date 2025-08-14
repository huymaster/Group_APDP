using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AssignedUser
{
    [Required] [MaxLength(36)] public required string UserId { get; set; }
    public User User { get; set; }

    [Required] [MaxLength(36)] public required string CourseId { get; set; }
    public Course Course { get; set; }
}