using Microsoft.EntityFrameworkCore;
using ToDoLIST.Api.Data;
using ToDoLIST.Api.Entities;
using ToDoLIST.Api.Interfaces;
using ToDoLIST.Api.Models;

namespace ToDoLIST.Api.Services;

public class ToDoItemService : IToDoItemService
{
    private readonly AppDbContext _context;

    public ToDoItemService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() => await _context.ToDoItems.ToListAsync();

    public async Task<ToDoItem?> GetByIdAsync(long id) => await _context.ToDoItems.FindAsync(id);

    public async Task<ToDoItem> CreateAsync(ToDoItem item)
    {
        item.CreatedAt = DateTime.UtcNow;
        _context.ToDoItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateAsync(long id, ToDoItem item)
    {
        var existing = await _context.ToDoItems.FindAsync(id);
        if (existing == null) return false;

        existing.Title = item.Title;
        existing.Description = item.Description;
        existing.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return false;
        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    // --- Maxsus Amallar ---

    public async Task<bool> CompleteAsync(long id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return false;
        item.IsCompleted = true;
        item.CompletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(long id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return false;
        item.IsDeleted = false; // yoki DeletedAt = null
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetPriorityAsync(long id, int priority)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return false;
        item.Priority = (PriorityLevel)priority;
        await _context.SaveChangesAsync();
        return true;
    }

    // --- Filtrlar ---

    public async Task<IEnumerable<ToDoItem>> GetCompletedAsync() =>
        await _context.ToDoItems.Where(t => t.IsCompleted).ToListAsync();

    public async Task<IEnumerable<ToDoItem>> GetPendingAsync() =>
        await _context.ToDoItems.Where(t => !t.IsCompleted).ToListAsync();

    public async Task<IEnumerable<ToDoItem>> GetOverdueAsync() =>
        await _context.ToDoItems.Where(t => t.DueDate < DateTime.UtcNow && !t.IsCompleted).ToListAsync();

    public async Task<IEnumerable<ToDoItem>> SearchAsync(string query) =>
        await _context.ToDoItems.Where(t => t.Title.Contains(query)).ToListAsync();

    public async Task<object> GetStatisticsAsync()
    {
        return new
        {
            Total = await _context.ToDoItems.CountAsync(),
            Completed = await _context.ToDoItems.CountAsync(t => t.IsCompleted),
            Pending = await _context.ToDoItems.CountAsync(t => !t.IsCompleted)
        };
    }
}

