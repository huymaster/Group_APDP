using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AssignedUser
{
    [Required]
    public User User { get; set; }
    
    [Required]
    public Course Course { get; set; }
}