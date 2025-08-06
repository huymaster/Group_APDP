using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AcmeChallengeController(ILogger<AcmeChallengeController> logger) : Controller
{
    [Route("~/.well-known/acme-challenge/{id}")]
    public ActionResult Index(string id)
    {
        var respond = $"{id}.U707BGvVXUtGx3hcVu6vm9ROJA1KYblnjry58JtQqUk";
        var client = HttpContext.Connection.RemoteIpAddress?.ToString();
        logger.LogInformation("ACME challange from {ip}", client);
        return Content(respond, "text/plain");
    }
}