using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Areas.Identity.Pages.Account;
using WebApp.Models;
using WebApp.Tests;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebAppTest;

public class LoginModelTests
{
    private readonly LoginModel _loginModel;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<UserManager<User>> _mockUserManager;

    public LoginModelTests()
    {
        _mockUserManager = MockHelper.MockUserManager();
        _mockSignInManager = MockHelper.MockSignInManager(_mockUserManager.Object);
        var mockLogger = new Mock<ILogger<LoginModel>>();

        _loginModel = new LoginModel(
            _mockSignInManager.Object,
            _mockUserManager.Object,
            mockLogger.Object
        );
    }

    [Fact]
    public async Task OnPostAsync_WithValidCredentials_RedirectsToReturnUrl()
    {
        var inputModel = new LoginModel.InputModel { Email = "test@example.com", Password = "Password123!" };
        _loginModel.Input = inputModel;
        _loginModel.ReturnUrl = "~/Dashboard";

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(inputModel.Email, inputModel.Password, false, false))
            .ReturnsAsync(SignInResult.Success);

        _mockUserManager.Setup(x => x.FindByNameAsync(inputModel.Email))
            .ReturnsAsync(new User { FullName = "Test User" });

        var result = await _loginModel.OnPostAsync(_loginModel.ReturnUrl);

        Assert.IsType<LocalRedirectResult>(result);
        var redirectResult = (LocalRedirectResult)result;
        Assert.Equal("~/Dashboard", redirectResult.Url);
    }

    [Fact]
    public async Task OnPostAsync_WithInvalidCredentials_AddsModelErrorAndReturnsPage()
    {
        var inputModel = new LoginModel.InputModel { Email = "wrong@example.com", Password = "wrongpassword" };
        _loginModel.Input = inputModel;

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Failed);

        var result = await _loginModel.OnPostAsync("");

        Assert.IsType<PageResult>(result);

        Assert.False(_loginModel.ModelState.IsValid);
        Assert.Equal("Invalid login attempt.", _loginModel.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_WhenLockedOut_RedirectsToLockoutPage()
    {
        var inputModel = new LoginModel.InputModel { Email = "locked@example.com", Password = "password" };
        _loginModel.Input = inputModel;

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.LockedOut);

        var result = await _loginModel.OnPostAsync("");

        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = (RedirectToPageResult)result;
        Assert.Equal("./Lockout", redirectResult.PageName);
    }
}