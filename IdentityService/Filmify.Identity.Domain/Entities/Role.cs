namespace Filmify.Identity.Domain.Entities;
public class Role
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Many-to-Many
    public List<User> Users { get; set; } = new();
}
