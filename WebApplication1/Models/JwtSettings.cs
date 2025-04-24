namespace JwtAuth.Models;

public class JwtSettings
{
    public String? Key { get; set; }
    public String? Issuer { get; set; }
    public String? Audience { get; set; }
}
