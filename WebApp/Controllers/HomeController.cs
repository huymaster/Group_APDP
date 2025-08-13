using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController(UserManager<User> authentication, ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}