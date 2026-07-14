using CarLog.Vehicle.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarLog.Vehicle.Api.Controllers;

#if DEBUG
[ApiController]
[Route("api/dev/token")]
public class DevTokenController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;

    public DevTokenController(JwtSettings jwtSettings) => _jwtSettings = jwtSettings;

    [HttpGet]
    public IActionResult GenerateToken() 
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);

        return Ok(new { tokern= new JwtSecurityTokenHandler().WriteToken(token)});
    }
}
#endif