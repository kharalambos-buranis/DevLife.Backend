using DevLife_Portal.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DevLife_Portal.Common.Services
{
    //public class TokenProvider
    //{
    //    private readonly JwtOptions _jwtOptions;

    //    public TokenProvider(IOptions<JwtOptions> jwtOptions)
    //    {
    //        _jwtOptions = jwtOptions.Value;
    //    }

    //    public string Create(User user)
    //    {
    //        string secretKey = _jwtOptions.Secret;

    //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    //        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(
    //            [
    //                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    //                new Claim("userId", user.Id.ToString()),
    //                new Claim("fullName", user.FullName),
    //                new Claim("username", user.Username)

    //            ]),

    //            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
    //            SigningCredentials = credentials,
    //            Issuer = _jwtOptions.Issuer,
    //            Audience = _jwtOptions.Audience
    //        };

    //        var handler = new JsonWebTokenHandler();

    //        string token = handler.CreateToken(tokenDescriptor);

    //        return token;
    //    }
    //}
}
