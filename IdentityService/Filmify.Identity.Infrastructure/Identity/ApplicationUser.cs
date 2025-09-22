using Filmify.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Filmify.Identity.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<long>
{
    public string FullName { get; set; } = "";

    public User ToDomain(List<Role>? roles = null)
    {
        return new User
        {
            Id = Id,
            FullName = FullName,
            Email = Email,
            Roles = roles ?? new()
        };
    }

    public static ApplicationUser FromDomain(User user)
    {
        return new ApplicationUser
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            UserName = user.Email
        };
    }
}

