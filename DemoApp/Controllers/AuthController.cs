using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApp.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    string signingKey = "BuBenimCokDahaUzunVeGuvenliSigningKeyDegerim0123456789";
    [HttpGet]
    public string Login(string userName, string password)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(JwtRegisteredClaimNames.Email, userName),
        };

        if (userName == "a@a.com")
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.Role, "User"));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var securityCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://topcubatuhan.com",
                audience: "BuBenimKullandigimAudienceDegeri",
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                notBefore: DateTime.Now,
                signingCredentials: securityCreds
            );
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }

    [HttpGet("ValidateToken")]
    public bool ValidateToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        try
        {
            JwtSecurityTokenHandler handler = new();
            handler.ValidateToken(token, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims.ToList();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
}
