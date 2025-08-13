using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using X.PagedList.Extensions;

namespace WebApp.Controllers;

public class CoursesController(ApplicationIdentityDbContext context, ILogger<CoursesController> logger) : Controller
{
    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Index(int? page)
    {
        var courses = context.Courses.Include(c => c!.Teacher).ToList();
        const int pageSize = 4;
        var pageNumber = courses.Count / pageSize + 1;
        var currentPage = page ?? 1;
        if (currentPage > pageNumber || currentPage < 1) return NotFound();
        var pagedList = courses.ToPagedList(currentPage, pageSize);

        return View(pagedList);
    }

    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Add()
    {
        return View();
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