using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DSW_3P_PR03.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR03.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDto login)
    {
        // Credenciales temporales para la práctica.
        if (login.Usuario != "admin" || login.Password != "ApocoLog123!")
        {
            return Unauthorized("Credenciales incorrectas");
        }

        var jwt = _configuration.GetSection("Jwt");
        var key = Convert.FromBase64String(jwt["Key"]!);
        var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes", 30);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Usuario),
            new Claim(ClaimTypes.Role, "Administrador")
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return Ok(new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(token),
            token_type = "Bearer",
            expires_in = expirationMinutes * 60
        });
    }
}