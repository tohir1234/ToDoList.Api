using Microsoft.EntityFrameworkCore;
using ToDoLIST.Api.Data;
using ToDoLIST.Api.Dtos;
using ToDoLIST.Api.Entities;
using ToDoLIST.Api.Interfaces;

namespace ToDoLIST.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();

    public async Task<User?> GetUserByIdAsync(long id) => await _context.Users.FindAsync(id);

    public async Task<bool> SetRoleAsync(long userId, string roleName)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
        var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == roleName);

        if (user == null || role == null) return false;

        user.RoleId = role.RoleId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserAsync(long id, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        user.Username = dto.Username;
        await _context.SaveChangesAsync();
        return true;
    }

   
}
