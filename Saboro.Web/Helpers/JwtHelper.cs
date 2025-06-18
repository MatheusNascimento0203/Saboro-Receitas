using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Saboro.Web.Helpers;

public static class JwtHelper
{
    public static string Create(string key, string value, double durationInHours)
    {
        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Hash, value)
            }),
            Expires = DateTime.UtcNow.AddHours(durationInHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature
            )
        });

        return handler.WriteToken(token);
    }

    public static string Read(string token, string key)
    {
        var claims = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true
        }, out SecurityToken validatedToken);

        var hashClaim = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash);
        if (hashClaim == null)
            return null;

        return hashClaim.Value;
    }
}
