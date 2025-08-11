using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;
[PrimaryKey("CourseCode")]
public class Course
{
    [Required]
    public string CourseCode { get; set; }
    [Required]
    public string CourseName { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public int Credits { get; set; }
    [Required]
    public  DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
    
    [Required]
    public User Teacher { get; set; }
    public ICollection<AssignedUser>  AssignedUsers { get; set; }
}