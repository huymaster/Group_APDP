using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class ApplicationIdentityDbContext(
    DbContextOptions<ApplicationIdentityDbContext> options
) : IdentityDbContext<User, UserRole, string>(options)
{
}