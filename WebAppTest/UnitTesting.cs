using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using Xunit.Abstractions;

namespace WebAppTest;

public class UnitTesting(PSQLFixture fixture, ITestOutputHelper helper) : IClassFixture<PSQLFixture>
{
    [Fact]
    public void TestGetDB()
    {
        var connectionString = fixture.GetConnectionString();
        helper.WriteLine(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<PSQLDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        using var context = new PSQLDbContext(optionsBuilder.Options);
        helper.WriteLine(context.ToString());
    }
}