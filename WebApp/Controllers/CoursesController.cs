using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;
using X.PagedList.Extensions;

namespace WebApp.Controllers;

public class CoursesController(ApplicationIdentityDbContext context) : Controller
{
    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Index(int? page)
    {
        var courses = context.Courses.ToList();
        const int pageSize = 4;
        var pageNumber = courses.Count / pageSize + 1;
        var currentPage = page ?? 1;
        if (currentPage > pageNumber || currentPage < 1) return NotFound();
        var pagedList = courses.ToPagedList(currentPage, pageSize);

        return View(pagedList);
    }

    [HttpPost]
    [Authorize(Policy = Policies.CanManageCourses)]
    public IActionResult Create(Course course)
    {
        context.Courses.Add(course);
        context.SaveChanges();
        return RedirectToAction("Index");
    }
}