using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using src.DriverRatings.Infrastructure.DTO;
using src.DriverRatings.Infrastructure.Extensions;
using src.DriverRatings.Infrastructure.Services.Interfaces;
using src.DriverRatings.Infrastructure.Settings;

namespace src.DriverRatings.Infrastructure.Services
{
  public class JwtHandler : IJwtHandler
  {
    public readonly AuthSettings _authSettings;

    public JwtHandler(AuthSettings authSettings)
    {
      this._authSettings = authSettings;
    }

    public JwtDto CreateToken(Guid userId, string role)
    {
      var now = DateTime.UtcNow;
      var id = userId.ToString();
      var claims = new Claim[] {
        new Claim(JwtRegisteredClaimNames.Sub, id),
        new Claim(JwtRegisteredClaimNames.UniqueName, id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
        new Claim(ClaimTypes.Role, role),
      };

      var expires = now.AddMinutes(_authSettings.ExpiryMinutes);
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._authSettings.Key)), SecurityAlgorithms.HmacSha256);
      var jwt = new JwtSecurityToken(issuer: this._authSettings.Issuer, claims: claims, notBefore: now, expires: expires, signingCredentials: signingCredentials);
      var token = new JwtSecurityTokenHandler().WriteToken(jwt);

      return new JwtDto
      {
        Token = token,
        Expires = expires.ToTimestamp(),
      };
    }
  }
}