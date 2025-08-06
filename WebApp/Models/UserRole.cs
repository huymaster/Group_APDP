using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class UserRole(string name) : IdentityRole(name)
{
    public override string? Name { get; set; }
    public override string? NormalizedName { get; set; }

    public void SetRole(Role role)
    {
        Name = Enum.GetName(role);
        NormalizedName = Name?.ToUpper();
    }

    public Role GetRole()
    {
        try
        {
            return Name == null ? Role.Student : Enum.Parse<Role>(Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Role.Student;
        }
    }
}

public enum Role
{
    Manager,
    Teacher,
    Student
}