using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Core;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
        var roleNames = Enum.GetNames(typeof(Role));

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
                await roleManager.CreateAsync(new UserRole(roleName));
        }
    }
}