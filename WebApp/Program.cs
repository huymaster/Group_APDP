using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var thumbprint = builder.Configuration["Kestrel:Certificates:Default:Thumbprint"];
    var cert = Certificate.GetCertificate(thumbprint);

    serverOptions.Listen(IPAddress.Any, 80);
    serverOptions.Listen(IPAddress.IPv6Any, 80);

    if (cert != null)
    {
        serverOptions.Listen(IPAddress.Any, 443, listenOptions => listenOptions.UseHttps(cert));
        serverOptions.Listen(IPAddress.IPv6Any, 443, listenOptions => listenOptions.UseHttps(cert));
    }
    else
    {
        Console.WriteLine("No certificate found. HTTPS will not be enabled.");
    }
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseExceptionHandler("/Error/InternalServerError");
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();
app.Run();