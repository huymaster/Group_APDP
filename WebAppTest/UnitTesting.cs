using WebApp.Models;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace WebAppTest;

public class UnitTesting(PSQLFixture fixture, ITestOutputHelper output) : IClassFixture<PSQLFixture>
{
    [Fact]
    [Order(0)]
    public void TestIdentityConnect()
    {
        using var context = fixture.GetTestApplicationIdentityDbContext();
        output.WriteLine(fixture.GetConnectionString());
        Assert.NotNull(context);
    }

    [Fact]
    [Order(1)]
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

        if (!context.Courses.Any(c => c.CourseCode == "AD1001"))
        {
            context.Courses.Add(course);
            context.SaveChanges();
        }

        Assert.Equal(1, context.Courses.Count());
    }

    [Fact]
    [Order(2)]
    public void TestFindCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        var found = context.Courses.Any(c => c.CourseCode == "AD1001");
        var course = context.Courses.FirstOrDefault(c => c.CourseCode == "AD1001");
        output.WriteLine("Found: " + found + " Course: " + course?.CourseId);
        Assert.True(found);
        Assert.NotNull(course);
    }
}