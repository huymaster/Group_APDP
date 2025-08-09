using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace WebApp.Data;

public class PSQLDbContext(
    DbContextOptions<PSQLDbContext> options,
    ILogger<PSQLDbContext>? logger = null
) : DbContext(options)
{
    private readonly ILogger<PSQLDbContext> logger = logger ?? new NullLogger<PSQLDbContext>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        logger.LogInformation("Creating model");
        base.OnModelCreating(modelBuilder);
        logger.LogInformation("Model created");
    }
}