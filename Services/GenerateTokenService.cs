using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserMenagerApi.Services;

public class GenerateTokenService : IGenerateTokenService
{
    public string GenerateToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my-very-secret-development-key-12345")); // Must match!
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, "1234567890"),
        new Claim(JwtRegisteredClaimNames.Name, "John Doe"),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}