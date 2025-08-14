using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;
using X.PagedList.Extensions;

namespace WebApp.Controllers;

public class CoursesController(
    ApplicationIdentityDbContext context,
    UserManager<User> userManager
) : Controller
{
    [Authorize(Policy = Policies.CanViewCourses)]
    public IActionResult Index(int? page)
    {
        var courses = context.Courses.Include(c => c.Teacher).ToList();
        const int pageSize = 4;
        var pageNumber = page ?? 1;
        var pagedList = courses.ToPagedList(pageNumber, pageSize);

        if (pagedList.PageNumber != pageNumber && page != null) return NotFound();

        return View(pagedList);
    }

    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Add()
    {
        var teachers = await userManager.GetUsersInRoleAsync(nameof(Role.Teacher));
        ViewBag.Teachers = new SelectList(teachers, "Id", "FullName");

        return View();
    }

    // Details
    [Authorize(Policy = Policies.CanViewCourses)]
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null) return NotFound();
        var course = await context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.CourseId == id);

        if (course == null) return NotFound();

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Add(
        [Bind("CourseName,CourseCode,Description,StartDate,EndDate,TeacherId")]
        Course course
    )
    {
        if (!ModelState.IsValid)
        {
            var teachers = await userManager.GetUsersInRoleAsync(nameof(Role.Teacher));
            ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", course.TeacherId);
            return View(course);
        }

        context.Add(course);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();

        var course = await context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        var teachers = await userManager.GetUsersInRoleAsync(nameof(Role.Teacher));
        ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", course.TeacherId);

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> Edit(string id,
        [Bind("CourseId,CourseName,CourseCode,Description,StartDate,EndDate,TeacherId")]
        Course course)
    {
        if (id != course.CourseId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(course);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Courses.Any(e => e.CourseId == course.CourseId)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var teachers = await userManager.GetUsersInRoleAsync(nameof(Role.Teacher));
        ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", course.TeacherId);

        return View(course);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageCourses)]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var course = await context.Courses.FindAsync(id);
        if (course == null) return RedirectToAction(nameof(Index));
        context.Courses.Remove(course);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}