using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Testcontainers.PostgreSql;
using WebApp.Data;

namespace WebAppTest;

public class PSQLFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("admin")
        .WithPassword("0")
        .WithCleanUp(false)
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>();
        optionsBuilder.UseCamelCaseNamingConvention();
        optionsBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsBuilder.UseNpgsql(connectionString);
        await using var context = new ApplicationIdentityDbContext(optionsBuilder.Options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return _container.GetConnectionString();
    }

    public DbContextOptions<TContext> GetTestDbOptions<TContext>() where TContext : DbContext
    {
        return new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(_container.GetConnectionString())
            .UseCamelCaseNamingConvention()
            .ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning))
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
            .UseCamelCaseNamingConvention()
            .Options;
    }

    public ApplicationIdentityDbContext GetTestApplicationIdentityDbContext()
    {
        return new ApplicationIdentityDbContext(GetTestDbOptions<ApplicationIdentityDbContext>());
    }

    public ApplicationIdentityDbContext GetRealApplicationIdentityDbContext(
        bool AreYouSureYouWantToUseRealDatabase = false)
    {
        return new ApplicationIdentityDbContext(
            GetRealDbOptions<ApplicationIdentityDbContext>(AreYouSureYouWantToUseRealDatabase));
    }
}