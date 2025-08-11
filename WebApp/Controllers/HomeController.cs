using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;

namespace WebApp.Controllers;

public class HomeController(ILogger<HomeController> logger, ApplicationIdentityDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;


    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = Policies.PublicResources)]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(Policy = Policies.ManagerOnly)]
    public async Task<IActionResult> UserList()
    {
        var users = await context.Users.ToListAsync();
        return View(users);
    }
}