namespace Filmify.Application.Common.Paging;

public class FilmSearchRequest : KeysetPagingRequest
{
    public string? SearchText { get; set; } 
}
