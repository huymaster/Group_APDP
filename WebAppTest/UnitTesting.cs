using Xunit.Abstractions;

namespace WebAppTest;

public class UnitTesting(PSQLFixture fixture, ITestOutputHelper output) : IClassFixture<PSQLFixture>
{
    [Fact]
    public void TestIdentityConnect()
    {
        using var context = fixture.GetTestApplicationIdentityDbContext();
        Assert.NotNull(context);
    }

    [Fact]
    public void TestAddCourse()
    {
    }
}