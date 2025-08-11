using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class AssignedUser
{
    [Required] public string UserId { get; set; }
    [NotMapped] public required User User { get; set; }

    [Required] public string CourseId { get; set; }
    [NotMapped] public required Course Course { get; set; }
}