using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WebApp.Areas.Identity.Pages.Account;
using WebApp.Data;
using WebApp.Models;

namespace WebAppTest;

public class LoginModelTests : IClassFixture<PSQLFixture>
{
    private readonly ApplicationIdentityDbContext _context;
    private readonly LoginModel _loginModel;
    private readonly UserManager<User> _userManager;

    public LoginModelTests(PSQLFixture fixture)
    {
        _context = fixture.GetRealApplicationIdentityDbContext(true);

        var userStore = new UserStore<User>(_context);

        var options = new Mock<IOptions<IdentityOptions>>();
        var identityOptions = new IdentityOptions();
        options.Setup(o => o.Value).Returns(identityOptions);
        var userValidators = new List<IUserValidator<User>>();
        var passwordValidators = new List<IPasswordValidator<User>>();
        var passwordHasher = new PasswordHasher<User>();

        _userManager = new UserManager<User>(
            userStore,
            options.Object,
            passwordHasher,
            userValidators,
            passwordValidators,
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null,
            new Mock<ILogger<UserManager<User>>>().Object
        );

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        var loggerSignInManager = new Mock<ILogger<SignInManager<User>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        var confirmation = new Mock<IUserConfirmation<User>>();

        var signInManager = new SignInManager<User>(
            _userManager,
            httpContextAccessor.Object,
            userPrincipalFactory.Object,
            options.Object,
            loggerSignInManager.Object,
            schemes.Object,
            confirmation.Object
        );

        var loggerLoginModel = new Mock<ILogger<LoginModel>>();

        _loginModel = new LoginModel(
            signInManager,
            _userManager,
            loggerLoginModel.Object
        );
    }

    [Fact]
    public async Task OnPostAsync_WithValidCredentials_RedirectsToReturnUrl()
    {
        var userEmail = "realuser@example.com";
        var userPassword = "Password123!";
        var user = new User { UserName = userEmail, Email = userEmail, FullName = "Real User" };
        await _userManager.CreateAsync(user, userPassword);

        _loginModel.Input = new LoginModel.InputModel { Email = userEmail, Password = userPassword };
        _loginModel.ReturnUrl = "~/Dashboard";

        var result = await _loginModel.OnPostAsync(_loginModel.ReturnUrl);

        Assert.IsType<LocalRedirectResult>(result);
        var redirectResult = (LocalRedirectResult)result;
        Assert.Equal("~/Dashboard", redirectResult.Url);

        var userToDelete = await _userManager.FindByEmailAsync(userEmail);
        if (userToDelete != null) await _userManager.DeleteAsync(userToDelete);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidCredentials_AddsModelErrorAndReturnsPage()
    {
        _loginModel.Input = new LoginModel.InputModel { Email = "wrong@example.com", Password = "wrongpassword" };

        var result = await _loginModel.OnPostAsync("");

        Assert.IsType<PageResult>(result);
        Assert.False(_loginModel.ModelState.IsValid);
        Assert.Equal("Invalid login attempt.", _loginModel.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WhenLockedOut_RedirectsToLockoutPage()
    {
        var userEmail = "lockeduser@example.com";
        var userPassword = "Password123!";
        var user = new User { UserName = userEmail, Email = userEmail, FullName = "Locked User" };
        await _userManager.CreateAsync(user, userPassword);

        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(10));

        _loginModel.Input = new LoginModel.InputModel { Email = userEmail, Password = userPassword };

        var result = await _loginModel.OnPostAsync("");

        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = (RedirectToPageResult)result;
        Assert.Equal("./Lockout", redirectResult.PageName);

        var userToDelete = await _userManager.FindByEmailAsync(userEmail);
        if (userToDelete != null) await _userManager.DeleteAsync(userToDelete);
    }
}