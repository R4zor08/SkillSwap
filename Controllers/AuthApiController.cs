using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Web.Controllers;

[ApiController]
[Route("api/auth")]
[Route("api/Login")]
public class AuthApiController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthApiController(IStudentService studentService, IJwtTokenService jwtTokenService)
    {
        _studentService = studentService;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>Register a new student and return a JWT for API calls (e.g. Postman).</summary>
    /// <remarks>Reachable as <c>POST /api/auth/register</c> and <c>POST /api/Login/Register</c> (ASP.NET Core route matching is case-insensitive).</remarks>
    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] AuthRegisterRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new
            {
                error = "Send JSON in the request body (not query parameters). Example: { \"name\": \"...\", \"email\": \"...\", \"password\": \"...\" } with header Content-Type: application/json."
            });

        if (_studentService.GetStudentByEmail(request.Email) != null)
            return Conflict(new { error = "Email is already registered." });

        var student = _studentService.CreateStudent(request.Name.Trim(), request.Email.Trim(), request.Password);
        var (token, expiresAt) = _jwtTokenService.CreateToken(student);
        return Ok(new TokenResponse
        {
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresAt = expiresAt,
            StudentId = student.StudentId,
            Email = student.Email,
            Name = student.Name
        });
    }

    /// <summary>Log in with email/password and return a JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] AuthLoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { error = "Email and password are required." });

        var student = _studentService.GetStudentByEmail(request.Email.Trim());
        if (student == null || !BCrypt.Net.BCrypt.Verify(request.Password, student.Password))
            return Unauthorized(new { error = "Invalid email or password." });

        var (token, expiresAt) = _jwtTokenService.CreateToken(student);
        return Ok(new TokenResponse
        {
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresAt = expiresAt,
            StudentId = student.StudentId,
            Email = student.Email,
            Name = student.Name
        });
    }

    /// <summary>Optional: call with Authorization: Bearer &lt;token&gt; to verify the JWT works.</summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(JwtRegisteredClaimNames.Email);
        var name = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var studentId))
            return Unauthorized();

        return Ok(new { studentId, email, name });
    }
}

public class AuthRegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthLoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class TokenResponse
{
    public string TokenType { get; set; } = "Bearer";
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Guid StudentId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
