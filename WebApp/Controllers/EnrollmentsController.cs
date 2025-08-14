using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class EnrollmentsController(
    ApplicationIdentityDbContext context,
    UserManager<User> userManager
) : Controller
{
    public IActionResult Index(string? courseId, string? returnUrl = null)
    {
        return RedirectToAction(nameof(Enroll), new { courseId, returnUrl });
    }

    [Authorize(Policy = Policies.CanAssignStudents)]
    public async Task<IActionResult> Enroll(string? courseId, string? returnUrl)
    {
        var courses = await context.Courses.ToListAsync();
        var course = courses.FirstOrDefault(c => c.CourseId == courseId);
        if (course == null) return NotFound();

        var assignedUsers = await context.AssignedUsers
            .Include(AU => AU.User)
            .Include(AU => AU.Course)
            .ToListAsync();
        var students = await context.Users.ToListAsync();
        var notEnrolled = new List<User>();

        foreach (var student in students.ToHashSet())
        {
            var isStudent = await userManager.IsInRoleAsync(student, nameof(Role.Student));
            if (!isStudent) continue;
            var isAssigned = assignedUsers.Any(a => a.UserId == student.Id && a.CourseId == courseId);
            if (isAssigned) continue;
            notEnrolled.Add(student);
        }

        ViewBag.Students = new SelectList(notEnrolled, "Id", "FullName");
        ViewBag.ReturnUrl = returnUrl;

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanAssignStudents)]
    public async Task<IActionResult> AssignStudent(
        [Bind("StudentId")] string? studentId, string? courseId, [Bind("ReturnUrl")] string? returnUrl)
    {
        if (courseId == null) return NotFound();
        var courses = await context.Courses.ToListAsync();
        var course = courses.FirstOrDefault(c => c.CourseId == courseId);
        if (course == null) return NotFound();
        if (studentId == null)
            ModelState.AddModelError("StudentId", "Student ID cannot be null");
        if (!ModelState.IsValid) return RedirectToAction(nameof(Enroll), new { courseId });

        var student = await userManager.FindByIdAsync(studentId);
        if (student == null)
        {
            ModelState.AddModelError("StudentId", "Student ID cannot be null");
            return RedirectToAction(nameof(Enroll), new { courseId });
        }

        if (!await userManager.IsInRoleAsync(student, nameof(Role.Student)))
        {
            ModelState.AddModelError("StudentId", "Student ID is not a valid student");
            return RedirectToAction(nameof(Enroll), new { courseId });
        }

        context.AssignedUsers.Add(new AssignedUser { UserId = studentId, CourseId = courseId });
        await context.SaveChangesAsync();
        if (returnUrl != null)
            return Redirect(returnUrl);
        return RedirectToAction(nameof(Index), new { courseId, returnUrl });
    }
}