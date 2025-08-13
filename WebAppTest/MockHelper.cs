using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using WebApp.Models;

namespace WebApp.Tests;

public static class MockHelper
{
    public static Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserEmailStore<User>>();

        var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        userManagerMock.Object.UserValidators.Add(new UserValidator<User>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<User>());

        userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        return userManagerMock;
    }

    public static Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        var signInManagerMock = new Mock<SignInManager<User>>(
            userManager,
            contextAccessor.Object,
            claimsFactory.Object,
            null, null, null, null
        );

        return signInManagerMock;
    }
}