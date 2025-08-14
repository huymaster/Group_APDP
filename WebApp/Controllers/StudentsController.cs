using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace WebApp.Controllers;

public class StudentsController(
    ApplicationIdentityDbContext context,
    UserManager<User> userManager,
    IPasswordHasher<User> passwordHasher,
    ILogger<StudentsController> logger
) : Controller
{
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IActionResult> Index(int? page)
    {
        var paged = await GetStudents(page);
        return View(paged);
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IPagedList<User>> GetStudents(int? page)
    {
        var users = context.Users.ToList();
        List<User> students = [];
        foreach (var user in users)
            if (await userManager.IsInRoleAsync(user, nameof(Role.Student)))
                students.Add(user);

        const int pageSize = 10;
        var pageNumber = Math.Min(1, students.Count / pageSize);

        var currentPage = page ?? 1;
        if (currentPage > pageNumber)
            currentPage = pageNumber;
        if (currentPage < 1)
            currentPage = 1;

        return students.ToPagedList(currentPage, pageSize);
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanManageStudents)]
    public IActionResult Add()
    {
        return View("AddStudent", Activator.CreateInstance<User>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IActionResult> Add(
        [Bind("FullName", "StudentCode", "Email", "PhoneNumber", "BirthDate")]
        User user
    )
    {
        if (string.IsNullOrEmpty(user.StudentCode))
            ModelState.AddModelError(nameof(user.StudentCode), "Student code is required.");
        else if (await context.Users.AnyAsync(u => u.StudentCode == user.StudentCode))
            ModelState.AddModelError(nameof(user.StudentCode), "Student code already exists.");

        validatePhoneNumber(user.PhoneNumber, ModelState, nameof(user.PhoneNumber));
        validateEmail(user.Email, ModelState, nameof(user.Email));
        if (string.IsNullOrEmpty(user.FullName))
            ModelState.AddModelError(nameof(user.FullName), "Full name is required.");
        if (!ModelState.IsValid)
            return View("AddStudent", user);

        user.UserName = user.Email;
        user.NormalizedEmail = user.Email.ToUpper();
        user.NormalizedUserName = user.Email.ToUpper();
        var result = await userManager.CreateAsync(user, "P@ssw0rd123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, nameof(Role.Student));
            user.EmailConfirmed = true;
            user.LockoutEnabled = true;
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(user, nameof(Role.Student));
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        ModelState.AddModelError(string.Empty, "Unknown error occurred.");
        return View("AddStudent", user);
    }

    private void validatePhoneNumber(string? PhoneNumber, ModelStateDictionary model, string key)
    {
        if (string.IsNullOrEmpty(PhoneNumber))
        {
            model.AddModelError(key, "Phone number is required.");
            return;
        }

        var pattern = "^0[0-9]{9}$";
        if (!Regex.IsMatch(PhoneNumber, pattern))
            model.AddModelError(key, "Phone number is invalid.");
    }

    private void validateEmail(string? Email, ModelStateDictionary model, string key)
    {
        if (string.IsNullOrEmpty(Email))
        {
            model.AddModelError(key, "Email is required.");
            return;
        }

        var pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        if (!Regex.IsMatch(Email, pattern))
            model.AddModelError(key, "Email is invalid.");
    }

    [HttpGet]
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IActionResult> Edit(string? id)
    {
        logger.LogInformation("Editing user {user}", id);
        if (string.IsNullOrEmpty(id))
            return NotFound();
        var users = await context.Users.ToListAsync();
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("StudentCode", "FullName", "Email", "PhoneNumber", "BirthDate")]
        User modUser
    )
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var users = await context.Users.ToListAsync();
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        if (string.IsNullOrEmpty(modUser.StudentCode))
            ModelState.AddModelError(nameof(modUser.StudentCode), "Student code is required.");
        else if (await context.Users.AnyAsync(u => u.StudentCode == modUser.StudentCode && u.Id != user.Id))
            ModelState.AddModelError(nameof(modUser.StudentCode), "Student code already exists.");

        validatePhoneNumber(modUser.PhoneNumber, ModelState, nameof(modUser.PhoneNumber));
        validateEmail(modUser.Email, ModelState, nameof(modUser.Email));
        if (string.IsNullOrEmpty(modUser.FullName))
            ModelState.AddModelError(nameof(modUser.FullName), "Full name is required.");
        if (!ModelState.IsValid)
            return View(modUser);

        if (modUser.FullName != user.FullName)
            user.FullName = modUser.FullName;

        if (await context.Users.AnyAsync(u => u.StudentCode == modUser.StudentCode))
            ModelState.AddModelError(nameof(modUser.StudentCode), "Student code already exists.");
        else if (!string.Equals(modUser.StudentCode, user.StudentCode, StringComparison.OrdinalIgnoreCase))
            user.StudentCode = modUser.StudentCode?.ToUpper() ?? "";

        if (modUser.NormalizedEmail != user.NormalizedEmail)
        {
            var email = await userManager.FindByEmailAsync(modUser.Email);
            if (email != null && email.Id != user.Id)
            {
                ModelState.AddModelError(nameof(modUser.Email), "Email already exists.");
            }
            else
            {
                user.Email = modUser.Email;
                user.NormalizedEmail = modUser.Email.ToUpper();
                user.UserName = modUser.Email;
                user.NormalizedUserName = modUser.Email.ToUpper();
            }
        }

        if (modUser.PhoneNumber != user.PhoneNumber) user.PhoneNumber = modUser.PhoneNumber;
        if (modUser.BirthDate != user.BirthDate) user.BirthDate = user.BirthDate;

        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded)
            return RedirectToAction(nameof(Index));

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(modUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Policies.CanManageStudents)]
    public async Task<IActionResult> Delete(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var user = await userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        await userManager.DeleteAsync(user);

        return RedirectToAction(nameof(Index));
    }
}