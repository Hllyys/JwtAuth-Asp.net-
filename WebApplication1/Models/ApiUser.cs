namespace JwtAuth.Models;

public class ApiUser
{
    public int Id {get; set; }
    public String? Username { get; set; }
    public String? Password { get; set; }
    public String? Role { get; set; }
}
