using Microsoft.AspNetCore.Authorization;
using WebApp.Models;

namespace WebApp.Core;

public static class PolicyDefinition
{
    private const string ROLE_MANAGER = nameof(Role.Manager);
    private const string ROLE_TEACHER = nameof(Role.Teacher);
    private const string ROLE_STUDENT = nameof(Role.Student);

    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.ManagerOnly, policy => policy.RequireRole(ROLE_MANAGER));
        options.AddPolicy(Policies.TeacherOnly, policy => policy.RequireRole(ROLE_TEACHER));
        options.AddPolicy(Policies.StudentOnly, policy => policy.RequireRole(ROLE_STUDENT));

        options.AddPolicy(Policies.PublicResources,
            policy => policy.RequireRole(ROLE_MANAGER, ROLE_TEACHER, ROLE_STUDENT));

        options.AddPolicy(Policies.CanManageTeachers, policy => policy.RequireRole(ROLE_MANAGER));
        options.AddPolicy(Policies.CanManageStudents, policy => policy.RequireRole(ROLE_MANAGER, ROLE_TEACHER));
        options.AddPolicy(Policies.CanManageRoles, policy => policy.RequireRole(ROLE_MANAGER));
        options.AddPolicy(Policies.CanAssignTeachers, policy => policy.RequireRole(ROLE_MANAGER));
        options.AddPolicy(Policies.CanManageCourses, policy => policy.RequireRole(ROLE_MANAGER, ROLE_TEACHER));
        options.AddPolicy(Policies.CanAssignStudents, policy => policy.RequireRole(ROLE_MANAGER, ROLE_TEACHER));
    }
}

public static class Policies
{
    public const string ManagerOnly = "ManagerOnly";
    public const string TeacherOnly = "TeacherOnly";
    public const string StudentOnly = "StudentOnly";
    public const string PublicResources = "PublicResources";
    public const string CanManageTeachers = "CanManageTeachers";
    public const string CanManageStudents = "CanManageStudents";
    public const string CanManageRoles = "CanManageRoles";
    public const string CanManageCourses = "CanManageCourses";
    public const string CanAssignTeachers = "CanAssignTeachers";
    public const string CanAssignStudents = "CanAssignStudents";
}