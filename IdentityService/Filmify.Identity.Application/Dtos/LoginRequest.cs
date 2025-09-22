namespace Filmify.Identity.Application.Dtos;

public record LoginRequest(
    string Email,
    string Password
);