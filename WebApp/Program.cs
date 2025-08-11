using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseNpgsql(connectionString).UseCamelCaseNamingConvention()
);

builder.Services.AddIdentity<User, UserRole>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(PolicyDefinition.ConfigurePolicies);

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

await using (var identityDbContext =
             app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>())
{
    await identityDbContext.Database.MigrateAsync();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await SeedData.Initialize(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseExceptionHandler("/Error/InternalServerError");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
);
app.UseMiddleware<Compressor>();

app.MapRazorPages();
app.Run();