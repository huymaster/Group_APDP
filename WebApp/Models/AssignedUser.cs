using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class AssignedUser
{
    [Required] [MaxLength(36)] public required string UserId { get; set; }
    [NotMapped] public required User User { get; set; }

    [Required] [MaxLength(36)] public required string CourseId { get; set; }
    [NotMapped] public required Course Course { get; set; }
}