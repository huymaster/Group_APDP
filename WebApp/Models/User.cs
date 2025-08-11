using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class User : IdentityUser
{
    [MaxLength(50)] public string? FullName { get; set; }

    public DateOnly BirthDate { get; set; }
}