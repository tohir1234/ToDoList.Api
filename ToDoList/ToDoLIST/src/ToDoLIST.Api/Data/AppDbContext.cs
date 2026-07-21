using Microsoft.EntityFrameworkCore;
using ToDoLIST.Api.Entities;
using ToDoLIST.Api.Models;

namespace ToDoLIST.Api.Data
{
    public class AppDbContext : DbContext
    {
        // To'g'ri konstruktor: u faqat optionlarni qabul qiladi
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User va Role o'rtasidagi bog'lanish
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // User va RefreshToken
            modelBuilder.Entity<User>()
                .HasOne(u => u.RefreshToken)
                .WithOne(r => r.User)
                .HasForeignKey<RefreshToken>(r => r.UserId);

            // User va ToDoItem
            modelBuilder.Entity<User>()
                .HasMany(u => u.ToDoItems)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}