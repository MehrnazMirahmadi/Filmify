namespace Filmify.Identity.Application.Dtos;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    List<string>? Roles = null
);
