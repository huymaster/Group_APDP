// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public IndexModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string Username { get; set; }

    [TempData] public string StatusMessage { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    private async Task LoadAsync(User user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            FullName = user.FullName,
            DateOfBirth = user.BirthDate == default ? DateOnly.FromDateTime(DateTime.Today) : user.BirthDate
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        if (Input.FullName != user.FullName && user.FullName != "[̲̅Đ][̲̅ạ][̲̅t] [̲̅0][̲̅9]" &&
            user.FullName != "kutoMadter"
           ) user.FullName = Input.FullName;

        if (Input.DateOfBirth != user.BirthDate) user.BirthDate = Input.DateOfBirth;

        var claims = await _userManager.GetClaimsAsync(user);

        var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
        if (fullNameClaim != null) await _userManager.RemoveClaimAsync(user, fullNameClaim);

        var birthDateClaim = claims.FirstOrDefault(c => c.Type == "BirthDate");
        if (birthDateClaim != null) await _userManager.RemoveClaimAsync(user, birthDateClaim);

        claims.Add(new Claim("FullName", user.FullName ?? ""));
        claims.Add(new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString("o")));

        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Full name")]
        [MaxLength(50)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [Age]
        public DateOnly DateOfBirth { get; set; }
    }

    internal class AgeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateOnly dateValue) return ValidationResult.Success;
            if (dateValue > DateOnly.FromDateTime(DateTime.Today))
                return new ValidationResult(ErrorMessage ?? "Date of birth must not be in the future.");
            if (dateValue < DateOnly.FromDateTime(DateTime.Today.AddYears(-100)))
                return new ValidationResult(ErrorMessage ?? "Date of birth must not be older than 100 years.");
            return ValidationResult.Success;
        }
    }

    internal class PhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (value is not string phoneNumber) return ValidationResult.Success;
            if (phoneNumber.Length != 10)
                return new ValidationResult("Phone number must be 10 digits long.");
            return !Regex.IsMatch(phoneNumber, pattern)
                ? new ValidationResult("Phone number is not valid.")
                : ValidationResult.Success;
        }
    }
}