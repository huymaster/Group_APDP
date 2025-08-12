using System.IO.Compression;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using WebApp.Core;
using WebApp.Data;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseNpgsql(connectionString).UseCamelCaseNamingConvention()
);

builder.Services.AddHealthChecks().AddNpgSql(connectionString);

builder.Services.AddIdentity<User, UserRole>(options => { options.SignIn.RequireConfirmedAccount = true; })
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    var brOptions = new BrotliCompressionProviderOptions
    {
        Level = CompressionLevel.SmallestSize
    };
    var gzipOptions = new GzipCompressionProviderOptions
    {
        Level = CompressionLevel.SmallestSize
    };
    options.Providers.Add(new BrotliCompressionProvider(brOptions));
    options.Providers.Add(new GzipCompressionProvider(gzipOptions));
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.SlidingExpiration = true;
    });
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

await using (
    var identityDbContext =
    app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>())
{
    await identityDbContext.Database.MigrateAsync();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await SeedData.Initialize(scope.ServiceProvider);
}

app.MapHealthChecks("/ServerHealth");

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/InternalServerError");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();
app.Run();