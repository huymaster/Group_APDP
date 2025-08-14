using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.ViewComponents;

public class NavBarViewComponent(UserManager<User> userManager) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var NavBarItems = new List<NavBarItem>();

        var userIdentity = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userIdentity ?? "");
        var roles = Enum.GetValues<Role>();
        var userRole = await userManager.GetRolesAsync(user ?? new User());
        var roleLevels = userRole
            .Select(Enum.Parse<Role>)
            .Select(r => (int)r)
            .ToList();
        var maxRoleLevel = roleLevels.Count == 0 ? -1 : roleLevels.Max();
        Role? role;
        if (maxRoleLevel == -1)
            role = null;
        else
            role = roles.FirstOrDefault(r => (int)r == maxRoleLevel);

        BuildNavBarItems(NavBarItems, role);
        return View(NavBarItems);
    }

    private void BuildNavBarItems(List<NavBarItem> NavBarItems, Role? role)
    {
        NavBarItems.Add("Home", "Home", "Index");
        switch (role)
        {
            case Role.Manager:
                BuildManagerItems(NavBarItems);
                break;
            case Role.Teacher:
                BuildTeacherItems(NavBarItems);
                break;
            case Role.Student:
                BuildStudentItems(NavBarItems);
                break;
        }
    }

    private static void BuildManagerItems(List<NavBarItem> NavBarItems)
    {
        NavBarItems.Add("Courses", "Courses", "Index");
        NavBarItems.Add("Teachers", "Teachers", "Index");
        NavBarItems.Add("Students", "Students", "Index");
    }

    private static void BuildTeacherItems(List<NavBarItem> NavBarItems)
    {
        NavBarItems.Add("Courses", "Courses", "Index");
    }

    private static void BuildStudentItems(List<NavBarItem> NavBarItems)
    {
        NavBarItems.Add("Courses", "Students", "View");
    }

    public struct NavBarItem
    {
        public static NavBarItem Create(string name, string controller, string action)
        {
            return Create(name, "", controller, action);
        }

        public static NavBarItem Create(string name, string area, string controller, string action)
        {
            return new NavBarItem { Name = name, Area = area, Controller = controller, Action = action };
        }

        public string Name { get; init; }
        public string Area { get; init; }
        public string Controller { get; init; }
        public string Action { get; init; }
    }
}

internal static class Extensions
{
    public static void Add(
        this List<NavBarViewComponent.NavBarItem> list,
        string name,
        string controller,
        string action
    )
    {
        list.Add(NavBarViewComponent.NavBarItem.Create(name, "", controller, action));
    }

    public static void Add(
        this List<NavBarViewComponent.NavBarItem> list,
        string name,
        string area,
        string controller,
        string action
    )
    {
        list.Add(NavBarViewComponent.NavBarItem.Create(name, area, controller, action));
    }
}