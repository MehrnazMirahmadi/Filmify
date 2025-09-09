namespace Filmify.Domain.Common;

public abstract class BaseEntity
{
    public DateTime RegDate { get; set; } = DateTime.UtcNow;
    public Guid RegisteringUserID { get; set; }
}
