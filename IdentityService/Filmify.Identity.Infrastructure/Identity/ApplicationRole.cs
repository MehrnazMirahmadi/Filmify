using Filmify.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Filmify.Identity.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<long>
{
    public string Description { get; set; } = "";

    public Role ToDomain()
    {
        return new Role
        {
            Id = Id,
            Name = Name,
            Description = Description
        };
    }

    public static ApplicationRole FromDomain(Role role)
    {
        return new ApplicationRole
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}