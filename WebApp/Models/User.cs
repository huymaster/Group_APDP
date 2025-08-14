using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class User : IdentityUser
{
    [Display(Name = "Full Name")]
    [MaxLength(50)]
    public string? FullName { get; set; }

    [Display(Name = "Student Code")]
    [MaxLength(12)]
    public string? StudentCode { get; set; }

    [Display(Name = "Date Of Birth")] public DateOnly BirthDate { get; set; }

    public ICollection<AssignedUser> AssignedUsers { get; set; } = new List<AssignedUser>();
}