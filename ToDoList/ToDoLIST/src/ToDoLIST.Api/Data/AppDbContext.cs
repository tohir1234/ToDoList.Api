using Microsoft.EntityFrameworkCore;
using ToDoLIST.Api.Entiti;
using ToDoLIST.Api.Models;

namespace ToDoLIST.Api.Data
{
    public class AppDbContext : DbContext
    {
        // To'g'ri konstruktor: u faqat optionlarni qabul qiladi
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet'lar klass ichida xususiyat (property) sifatida e'lon qilinadi
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}