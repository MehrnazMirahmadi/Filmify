namespace Filmify.Identity.Application.Dtos;
public record AuthResponse(
    string Token,
    DateTime Expiration
);