using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDoLIST.Api.Data;
using ToDoLIST.Api.Interfaces; // Nomini to'g'rilang!

using ToDoLIST.Api.Services;

namespace ToDoLIST.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Ma'lumotlar bazasi
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. JWT Autentifikatsiya (Token tekshiruvi)
            var jwtKey = builder.Configuration["Jwt:Key"] ?? "MaxfiyKalitSozYozingBuYerga123!";
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // 3. Servislarni ro'yxatdan o'tkazish (DI)
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

            // 4. AutoMapper va Controller'lar
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // HTTP so'rovlar quvuri (Pipeline)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Middleware ketma-ketligi juda muhim!
            app.UseAuthentication(); // 1. Kimligini bil
            app.UseAuthorization();  // 2. Ruxsati bormi tekshir

            app.MapControllers();

            app.Run();
        }
    }
}