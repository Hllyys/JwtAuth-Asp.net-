using System.Text;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Uygulamanýn kimlik doðrulama þemasý olarak JWT Bearer'ý kullanacaðýný belirtir.
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'ý yayýmlayan (issuer) bilgisi doðrulansýn mý? Genelde true yapýlýr.
            ValidateAudience = true, // Token'ýn hedef kitlesi (audience) doðrulansýn mý? Genelde true yapýlýr.
            ValidateLifetime = true, // Token'ýn süresi dolmuþ mu diye kontrol edilir. Bu da genellikle true olur.
            ValidateIssuerSigningKey = true, // Token'ýn imzasý doðrulansýn mý? Güvenlik açýsýndan true olmasý gerekir.

            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Token'ýn olmasý gereken yayýmlayýcý deðeri. appsettings.json içinde "Jwt:Issuer" kýsmýnda tanýmlanýr.
            ValidAudience = builder.Configuration["Jwt:Audience"], // Token'ýn olmasý gereken hedef kitle deðeri. appsettings.json içinde "Jwt:Audience" kýsmýnda tanýmlanýr.

            // Token'ý imzalamak için kullanýlan gizli anahtar. Güvenli bir þekilde saklanmalý.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))//ppsettings.json içindeki "Jwt:Key" deðerini UTF8 olarak byte dizisine çevirir.
        };
    });
// appsettings.json dosyasýndaki "Jwt" isimli konfigürasyon bölümünü,
// JwtSettings sýnýfýna map'leyerek DI (Dependency Injection) sistemi ile kullanýlabilir hale getirir.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
