using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace WebAppTest;

public class UnitTesting(PSQLFixture fixture, ITestOutputHelper output) : IClassFixture<PSQLFixture>
{
    [Fact]
    [Order(0)]
    public void TestRIdentityConnect()
    {
        using var c = fixture.GetRealApplicationIdentityDbContext(true);
        output.WriteLine(fixture.GetConnectionString());
        Assert.NotNull(c);
    }

    [Fact]
    [Order(0)]
    public void TestIdentityConnect()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        output.WriteLine(fixture.GetConnectionString());
        Assert.NotNull(context);
    }

    [Fact]
    [Order(1)]
    public void TestAddCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        Course c = new()
        {
            CourseCode = "AD1001",
            CourseName = "Android Development",
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2022, 4, 1),
            Credits = 5
        };

        context.Courses.Add(c);
        context.SaveChanges();

        Assert.Equal(1, context.Courses.Count());
    }

    [Fact]
    [Order(2)]
    public void TestFindCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        context.Courses.Load();
        var course = context.Courses.FirstOrDefault(c => c.CourseCode == "AD1001");
        Assert.Equal("AD1001", course.CourseCode);
    }

    [Fact]
    [Order(4)]
    public void TestUpdateCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        context.Courses.Load();
        var course = context.Courses.First();
        course.CourseCode = "AD1002";
        context.SaveChanges();
        Assert.Equal("AD1002", course.CourseCode);
    }

    [Fact]
    [Order(5)]
    public void TestDeleteCourse()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        context.Courses.Load();
        var course = context.Courses.First();
        context.Courses.Remove(course);
        context.SaveChanges();
        Assert.Equal(0, context.Courses.Count());
    }

    [Fact]
    [Order(6)]
    public void TestCleanUp()
    {
        var context = fixture.GetTestApplicationIdentityDbContext();
        context.Courses.Load();
        context.Courses.RemoveRange(context.Courses);
        context.SaveChanges();
        Assert.Equal(0, context.Courses.Count());
    }
}