using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationIdentityDbContext(
    DbContextOptions<ApplicationIdentityDbContext> options,
    ILogger<ApplicationIdentityDbContext> logger
) : IdentityDbContext<User, UserRole, string>(options)
{
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