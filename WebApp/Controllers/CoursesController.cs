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
        var courses = context.Courses
            .Include(c => c.AssignedUsers)
            .Include(c => c.Teacher).ToList();
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

    [HttpGet]
    [Authorize(Policy = Policies.CanViewCourses)]
    public async Task<IActionResult> GetStudents(string? courseId)
    {
        ViewData["CourseId"] = courseId;
        if (courseId == null) return PartialView("_StudentList", null);
        var course = await context.Courses.FindAsync(courseId);
        if (course == null) return PartialView("_StudentList", null);
        var students = await context.AssignedUsers
            .Include(u => u.User)
            .Where(u => u.CourseId == courseId)
            .Select(u => u.User)
            .ToListAsync();

        return PartialView("_StudentList", students);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanAssignStudents)]
    public async Task<IActionResult> RemoveFromCourse(string studentId, string courseId)
    {
        var courses = await context.Courses.ToListAsync();
        var course = courses.FirstOrDefault(c => c.CourseId == courseId);
        if (course == null)
            ModelState.AddModelError(string.Empty, "Course not found");
        var students = await context.Users.ToListAsync();
        var student = students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
            ModelState.AddModelError(string.Empty, "Student not found");

        var assignedUsers = await context.AssignedUsers.Include(au => au.User).Include(au => au.Course).ToListAsync();
        var assignedUser = assignedUsers.FirstOrDefault(au => au.CourseId == courseId && au.UserId == studentId);
        if (assignedUser == null || true)
            ModelState.AddModelError(string.Empty, "Student not assigned to course");

        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Details), new { id = courseId });

        if (assignedUser != null)
            context.AssignedUsers.Remove(assignedUser);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = courseId });
    }
}