using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;
using X.PagedList.Extensions;

namespace WebApp.Controllers;

public class CoursesController(ApplicationIdentityDbContext context, ILogger<CoursesController> logger) : Controller
{
    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Index(int? page)
    {
        var courses = context.Courses.Include(c => c!.Teacher).ToList();
        const int pageSize = 4;
        var pageNumber = page ?? 1;
        var pagedList = courses.ToPagedList(pageNumber, pageSize);

        if (pagedList.PageNumber != pageNumber && page != null) return NotFound();

        return View(pagedList);
    }

    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Add(
        [Bind("CourseName,CourseCode,Description,StartDate,EndDate,TeacherId")]
        Course course)
    {
        if (!ModelState.IsValid) return View(course);
        course.CourseId = Guid.NewGuid().ToString();
        context.Add(course);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Edit(string? id)
    {
        logger.LogInformation("Edit course " + id);
        if (id == null) return NotFound();
        var course = await context.Courses.Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.CourseId == id);
        if (course == null) return NotFound();
        return View(course);
    }
}