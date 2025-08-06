using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AccountController(ILogger<HomeController> logger) : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> GoogleAuth([FromServices] IGoogleAuthProvider provider)
    {
        var result = await provider.GetCredentialAsync();
        Console.WriteLine(result?.UnderlyingCredential.GetAccessTokenForRequestAsync());
        return View();
    }
}