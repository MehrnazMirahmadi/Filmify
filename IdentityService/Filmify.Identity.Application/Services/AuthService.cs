using Filmify.Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Filmify.Identity.Application.Services;

public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    //
    public async Task RegisterUserAsync(string fullName, string email, string password, List<string>? roles = null)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null) throw new Exception("User already exists");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

      
        if (roles != null && roles.Any())
        {
            foreach (var roleName in roles)
            {
              
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }

 
    public async Task<ApplicationUser?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var valid = await _userManager.CheckPasswordAsync(user, password);
        return valid ? user : null;
    }

   
    public async Task AssignRolesAsync(long userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new Exception("User not found");

        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            if (!await _userManager.IsInRoleAsync(user, roleName))
                await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
