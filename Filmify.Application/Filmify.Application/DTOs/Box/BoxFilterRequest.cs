using Filmify.Application.Common.Paging;

namespace Filmify.Application.DTOs.Box;

public class BoxFilterRequest : KeysetPagingRequest
{
    public string? SearchText { get; set; } //  BoxName
}