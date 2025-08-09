using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Controllers;

public class HomeController(ILogger<HomeController> logger, ApplicationIdentityDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;


    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}