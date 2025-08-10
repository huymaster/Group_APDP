using System.Security;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using WebApp.Data;

namespace WebAppTest;

public class PSQLFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("testdb")
        .WithUsername("testuser")
        .WithPassword("testpassword")
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<PSQLDbContext>();
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(connectionString);
        await using var context = new PSQLDbContext(optionsBuilder.Options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public DbContextOptions<TContext> GetTestDbOptions<TContext>() where TContext : DbContext
    {
        return new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(_container.GetConnectionString())
            .UseSnakeCaseNamingConvention()
            .Options;
    }

    public DbContextOptions<TContext> GetRealDbOptions<TContext>(bool AreYouSureYouWantToUseRealDatabase = false)
        where TContext : DbContext
    {
        if (!AreYouSureYouWantToUseRealDatabase) throw new SecurityException("!!! You're using the real database !!!");
        const string connectionString =
            "Host=studentmanager.ddns.net;Port=5432;Username=web_client;Password=123456;Database=postgres;SearchPath=student_manager";
        return new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;
    }
}