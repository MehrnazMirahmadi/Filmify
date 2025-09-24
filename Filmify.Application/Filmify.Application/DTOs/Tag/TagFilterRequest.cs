using Filmify.Application.Common.Paging;

namespace Filmify.Application.DTOs.Tag;

public class TagFilterRequest : KeysetPagingRequest
{
    public string? SearchText { get; set; } //  TagText
}