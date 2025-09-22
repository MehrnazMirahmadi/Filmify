using Filmify.Identity.Domain.Entities;

namespace Filmify.Identity.Domain.Contracts.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(long id);
    Task AddAsync(User user, string password, List<string>? roleNames = null);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<List<Role>> GetUserRolesAsync(long userId);
    Task AssignRolesAsync(long userId, List<string> roleNames);
}