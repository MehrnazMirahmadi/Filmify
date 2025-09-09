using Filmify.Domain.Common;

namespace Filmify.Domain.Entities;

public class Tag : BaseEntity
{
    public int TagID { get; set; }
    public string TagText { get; set; } = null!;
    public DateTime RegDate { get; set; } = DateTime.UtcNow;

    public Guid RegisteringUserID { get; set; }
    public Guid? ApprovalUserID { get; set; }


    public ICollection<FilmTag> FilmTags { get; set; } = new List<FilmTag>();
}
