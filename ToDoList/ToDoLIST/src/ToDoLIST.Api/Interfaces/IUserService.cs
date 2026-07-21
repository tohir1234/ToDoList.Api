using ToDoLIST.Api.Dtos;
using ToDoLIST.Api.Entities;

namespace ToDoLIST.Api.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(long id);
    Task<bool> SetRoleAsync(long userId, string roleName);
    Task<bool> DeleteUserAsync(long id);
    Task<bool> UpdateUserAsync(long id, UserUpdateDto dto);
}
