using System.Text;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Uygulaman�n kimlik do�rulama �emas� olarak JWT Bearer'� kullanaca��n� belirtir.
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'� yay�mlayan (issuer) bilgisi do�rulans�n m�? Genelde true yap�l�r.
            ValidateAudience = true, // Token'�n hedef kitlesi (audience) do�rulans�n m�? Genelde true yap�l�r.
            ValidateLifetime = true, // Token'�n s�resi dolmu� mu diye kontrol edilir. Bu da genellikle true olur.
            ValidateIssuerSigningKey = true, // Token'�n imzas� do�rulans�n m�? G�venlik a��s�ndan true olmas� gerekir.

            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Token'�n olmas� gereken yay�mlay�c� de�eri. appsettings.json i�inde "Jwt:Issuer" k�sm�nda tan�mlan�r.
            ValidAudience = builder.Configuration["Jwt:Audience"], // Token'�n olmas� gereken hedef kitle de�eri. appsettings.json i�inde "Jwt:Audience" k�sm�nda tan�mlan�r.

            // Token'� imzalamak i�in kullan�lan gizli anahtar. G�venli bir �ekilde saklanmal�.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))//ppsettings.json i�indeki "Jwt:Key" de�erini UTF8 olarak byte dizisine �evirir.
        };
    });
// appsettings.json dosyas�ndaki "Jwt" isimli konfig�rasyon b�l�m�n�,
// JwtSettings s�n�f�na map'leyerek DI (Dependency Injection) sistemi ile kullan�labilir hale getirir.
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
