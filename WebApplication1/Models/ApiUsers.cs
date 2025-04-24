namespace JwtAuth.Models;

public class ApiUsers//veritabanı olarak kullanılacak
{
    public static List<ApiUser> Users = new List<ApiUser>()
    {
        new ApiUser {Id=1,Username="hilal",Password="123456",Role="Admin"},
        new ApiUser {Id=2,Username="elif",Password="123456",Role="StandartUser"}
    };

}
