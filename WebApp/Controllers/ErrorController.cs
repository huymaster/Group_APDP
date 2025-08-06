using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ErrorController(ILogger<ErrorController> logger) : Controller
{
    public new IActionResult NotFound()
    {
        return View();
    }

    public IActionResult InternalServerError()
    {
        var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var ex = exceptionHandler?.Error;
        return View(ex);
    }
}