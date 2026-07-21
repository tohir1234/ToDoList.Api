using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoLIST.Api.Data;   // <-- DbContext turgan papka nomi (agar xato bersa, o'zingiznikiga moslang)
using ToDoLIST.Api.Dtos;   // <-- DTO'lar turgan papka nomi
using ToDoLIST.Api.Entities;
using ToDoLIST.Api.Interfaces; // <-- Entitilar (User, UserRole) turgan papka nomi

namespace ToDoLIST.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(UserRegisterDto registerDto)
        {
            // 1. Username band yoki band emasligini tekshirish
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return "Username allaqachon band!";
            }

            // 2. Bazada standart "User" roli borligini tekshirish (yo'q bo'lsa yaratish)
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                userRole = new UserRole { Name = "User" };
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();
            }

            // 3. Parolni shifrlash (Hash)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // 4. Yangi foydalanuvchini yaratish
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = hashedPassword,
                RoleId = userRole.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Muvaffaqiyatli ro'yxatdan o'tdingiz!";
        }

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto loginDto)
        {
            // 1. Foydalanuvchini tekshirish
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null) return null;

            // 2. Parolni solishtirish (BCrypt orqali)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid) return null;

            // 3. JWT Token yaratish
            var accessToken = GenerateJwtToken(user);

            // 4. Refresh Token yaratish va bazada yangilash
            var refreshToken = GenerateRefreshToken(user.UserId);

            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == user.UserId);
            if (existingToken != null)
            {
                existingToken.Token = refreshToken.Token;
                existingToken.ExpiresAt = refreshToken.ExpiresAt;
                _context.RefreshTokens.Update(existingToken);
            }
            else
            {
                _context.RefreshTokens.Add(refreshToken);
            }

            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        // JWT Token yaratuvchi yordamchi metod
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // Refresh Token yaratuvchi yordamchi metod
        private RefreshToken GenerateRefreshToken(long userId)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Token 7 kun yashaydi
                UserId = userId
            };
        }
    }
}