using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}