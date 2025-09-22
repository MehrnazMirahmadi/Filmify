namespace Filmify.Identity.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";

    // Many-to-Many
    public List<Role> Roles { get; set; } = new();
}
