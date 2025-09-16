using Filmify.Domain.Common;

namespace Filmify.Domain.Entities;

public class Category : BaseEntity
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = null!;

    // Navigation Property
    public ICollection<Film> Films { get; set; } = new List<Film>();
}