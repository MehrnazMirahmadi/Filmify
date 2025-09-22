using Filmify.Identity.Domain.Contracts.Interfaces;
using Filmify.Identity.Domain.Entities;
using Filmify.Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Filmify.Identity.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task AddAsync(User user, string password, List<string>? roleNames = null)
    {
        var appUser = ApplicationUser.FromDomain(user);
        var result = await _userManager.CreateAsync(appUser, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

        if (roleNames != null && roleNames.Count > 0)
        {
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

                await _userManager.AddToRoleAsync(appUser, roleName);
            }
        }
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null) return null;

        var roles = await GetUserRolesAsync(appUser.Id);
        return appUser.ToDomain(roles);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null) return null;

        var roles = await GetUserRolesAsync(appUser.Id);
        return appUser.ToDomain(roles);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var appUser = await _userManager.FindByEmailAsync(user.Email);
        if (appUser == null) return false;
        return await _userManager.CheckPasswordAsync(appUser, password);
    }

    public async Task<List<Role>> GetUserRolesAsync(long userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null) return new List<Role>();

        var roleNames = await _userManager.GetRolesAsync(appUser);
        var roles = new List<Role>();
        foreach (var name in roleNames)
        {
            var appRole = await _roleManager.FindByNameAsync(name);
            if (appRole != null)
                roles.Add(appRole.ToDomain());
        }
        return roles;
    }

    public async Task AssignRolesAsync(long userId, List<string> roleNames)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null) throw new Exception("User not found");

        var currentRoles = await _userManager.GetRolesAsync(appUser);
        await _userManager.RemoveFromRolesAsync(appUser, currentRoles);

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            await _userManager.AddToRoleAsync(appUser, roleName);
        }
    }
}