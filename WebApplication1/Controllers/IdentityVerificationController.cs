using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles ="Admin,StandartUser")]
public class IdentityVerificationController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    public IdentityVerificationController(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }


    [AllowAnonymous]//girişi auth dan ayrılsın herkes girebilsin
    [HttpPost("Login")]
    public IActionResult Login([FromBody] ApiUser apiUserInfo)//veritabanı ile karşılaştırır
    {
        var apiUser = UserIdentity(apiUserInfo);
        if (apiUser == null) return NotFound("Kulanıcı Bulunamadı.");
        var token = CreateToken(apiUser);
        return Ok(token);

    }

    private String CreateToken(ApiUser apiUser)
    {
     
        if (_jwtSettings.Key == null) throw new Exception("Jwt ayarlarındaki Key değeri null olamaz.");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsSeries = new[]//token içinde istediğimişz bilgiler(herkes görebilir)
        {
           new Claim(ClaimTypes.NameIdentifier, apiUser.Username!),
           new Claim(ClaimTypes.Role,apiUser.Role!)
        };

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claimsSeries,
            expires: DateTime.Now.AddHours(1),
            signingCredentials:credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ApiUser? UserIdentity(ApiUser apiUserInfo)
    {
        return ApiUsers
            .Users
            .FirstOrDefault(
            x => x.Username?.ToLower() == apiUserInfo.Username
            && x.Password ==apiUserInfo.Password
            );
    }
}
