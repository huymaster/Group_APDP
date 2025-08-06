using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Controllers;

public class HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        context.Database.EnsureCreated();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}