using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using WebApp.Data;

namespace WebAppTest;

public class PSQLFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("testuser")
        .WithPassword("testpassword")
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<PSQLDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        await using var context = new PSQLDbContext(optionsBuilder.Options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return _container.GetConnectionString();
    }
}