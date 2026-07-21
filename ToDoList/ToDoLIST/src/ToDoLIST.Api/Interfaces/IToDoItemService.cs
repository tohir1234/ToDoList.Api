using ToDoLIST.Api.Entities;

namespace ToDoLIST.Api.Interfaces;

public interface IToDoItemService
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(long id);
    Task<ToDoItem> CreateAsync(ToDoItem item);
    Task<bool> UpdateAsync(long id, ToDoItem item);
    Task<bool> DeleteAsync(long id);
    // Maxsus amallar
    Task<bool> CompleteAsync(long id);
    Task<bool> RestoreAsync(long id);
    Task<bool> SetPriorityAsync(long id, int priority);
    // Filtrlar
    Task<IEnumerable<ToDoItem>> GetCompletedAsync();
    Task<IEnumerable<ToDoItem>> GetPendingAsync();
    Task<IEnumerable<ToDoItem>> GetOverdueAsync();
    Task<IEnumerable<ToDoItem>> SearchAsync(string query);
    Task<object> GetStatisticsAsync();
}
