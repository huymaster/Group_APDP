using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WebApp.Models;

namespace WebApp.Data;

public class PSQLDbContext(
    DbContextOptions<PSQLDbContext> options,
    ILogger<PSQLDbContext>? logger = null
) : DbContext(options)
{
    private readonly ILogger<PSQLDbContext> logger = logger ?? new NullLogger<PSQLDbContext>();
    
    public DbSet<Course> Courses { get; set; }
    public DbSet<AssignedUser> AssignedUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        logger.LogInformation("Creating model");
        base.OnModelCreating(modelBuilder);
        CreateTables(modelBuilder);
        logger.LogInformation("Model created");
    }

    private static void CreateTables(ModelBuilder builder)
    {
    }
}