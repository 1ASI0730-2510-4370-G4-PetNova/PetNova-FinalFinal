using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetNova.API.Shared.Application.Services;
using PetNova.API.Veterinary.IAM.Application.Services;
using PetNova.API.Veterinary.IAM.Interface.DTOs;

namespace PetNova.API.Veterinary.IAM.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(AuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        if (user == null) 
            return BadRequest("Username or email already exists");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { user, token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        var user = await _authService.AuthenticateAsync(loginDto);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { user, token });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _authService.ListUsersAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _authService.GetUserByIdAsync(Guid.Parse(userId));
        if (user == null) return NotFound();

        return Ok(user);
    }
}