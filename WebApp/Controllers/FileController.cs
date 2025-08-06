using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class FileController : Controller
{
    public IActionResult DownloadAPK()
    {
        var bytes = System.IO.File.ReadAllBytes("wwwroot/files/Android.apk");
        return File(bytes, "application/vnd.android.package-archive", "CampusExpenseManager.apk");
    }
}