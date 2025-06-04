using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users.Application.Interfaces.Authentication;
using Users.Core.Enums;
using Users.Core.Models;

namespace Users.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(User user, IEnumerable<Role> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email.Value)
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,  
            audience: _options.Audience,  
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
            signingCredentials: GetSigningCredentials());

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
    
    private SigningCredentials GetSigningCredentials() =>
        new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256);
    
    private SymmetricSecurityKey GetSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
}