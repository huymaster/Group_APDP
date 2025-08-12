using WebApp.Models;
using Xunit.Abstractions;

namespace WebAppTest;

public class UnitTesting(PSQLFixture fixture, ITestOutputHelper output) : IClassFixture<PSQLFixture>
{
    [Fact]
    public void TestIdentityConnect()
    {
        using var context = fixture.GetTestApplicationIdentityDbContext();
        output.WriteLine(fixture.GetConnectionString());
        Assert.NotNull(context);
    }

    [Fact]
    public void TestAddCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        var course = new Course
        {
            CourseCode = "AD1001",
            CourseName = "Android Development",
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2022, 4, 1),
            Credits = 5,
            Teacher = new User()
        };

        context.Courses.Add(course);
        context.SaveChanges();

        Assert.Equal(1, context.Courses.Count());
        Assert.True(context.Courses.Any(c => c.CourseCode == "AD1001"));
    }
}