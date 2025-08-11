using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationIdentityDbContext(
    DbContextOptions<ApplicationIdentityDbContext> options,
    ILogger<ApplicationIdentityDbContext>? logger = null
) : IdentityDbContext<User, UserRole, string>(options)
{
    private readonly ILogger<ApplicationIdentityDbContext> _logger =
        logger ?? new NullLogger<ApplicationIdentityDbContext>();

    public DbSet<Course> Courses { get; set; }
    public DbSet<AssignedUser> AssignedUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation("Creating model");
        base.OnModelCreating(modelBuilder);
        CreateTables(modelBuilder);
        _logger.LogInformation("Model created");
    }

    private static void CreateTables(ModelBuilder builder)
    {
        builder.Entity<AssignedUser>()
            .HasKey(e => new { e.UserId, e.CourseId });

        builder.Entity<AssignedUser>()
            .HasOne(e => e.User)
            .WithMany(e => e.AssignedUsers)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.Entity<AssignedUser>()
            .HasOne(e => e.Course)
            .WithMany(e => e.AssignedUsers)
            .HasForeignKey(e => e.CourseId)
            .IsRequired();
    }
}