using Filmify.Identity.Application.Dtos;
using Filmify.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

 
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Validation 
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and Password are required.");

        try
        {
            await _authService.RegisterUserAsync(
                request.FullName,
                request.Email,
                request.Password,
                request.Roles
            );

            return Created("", new { Message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// User Login and Get JWT
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and Password are required.");

        var user = await _authService.LoginAsync(request.Email, request.Password);
        if (user == null) return Unauthorized("Invalid credentials");

       
        var (token, expiration) = await _jwtService.GenerateTokenAsync(user);
        return Ok(new { Token = token, Expiration = expiration });

    }

   
    [HttpPost("assign-roles/{userId}")]
    public async Task<IActionResult> AssignRoles(long userId, [FromBody] List<string> roles)
    {
        try
        {
            await _authService.AssignRolesAsync(userId, roles);
            return Ok(new { Message = "Roles assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
