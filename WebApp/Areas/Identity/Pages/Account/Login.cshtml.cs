#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty] public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData] public string ErrorMessage { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(Input.Email);
                if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

                var claims = new List<Claim>
                {
                    new("FullName", user.FullName ?? ""),
                    new(ClaimTypes.DateOfBirth, user.BirthDate.ToString("o"))
                };
                await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, claims);
                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa",
                    new { ReturnUrl = returnUrl, Input.RememberMe });

            if (result.IsLockedOut) return RedirectToPage("./Lockout");

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return Page();
    }

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
    }
}