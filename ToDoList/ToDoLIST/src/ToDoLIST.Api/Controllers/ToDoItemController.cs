using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoLIST.Api.Entities;
using ToDoLIST.Api.Interfaces;

namespace ToDoLIST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _service;
        public ToDoItemController(IToDoItemService service) => _service = service;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> GetById(long id) => Ok(await _service.GetByIdAsync(id));
        [HttpPost] public async Task<IActionResult> Create(ToDoItem item) => Ok(await _service.CreateAsync(item));
        [HttpPut("{id}")] public async Task<IActionResult> Update(long id, ToDoItem item) => Ok(await _service.UpdateAsync(id, item));
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(long id) => Ok(await _service.DeleteAsync(id));

        // Maxsus amallar
        [HttpPut("{id}/complete")] public async Task<IActionResult> Complete(long id) => Ok(await _service.CompleteAsync(id));
        [HttpPut("{id}/restore")] public async Task<IActionResult> Restore(long id) => Ok(await _service.RestoreAsync(id));
        [HttpPatch("{id}/priority")] public async Task<IActionResult> SetPriority(long id, int priority) => Ok(await _service.SetPriorityAsync(id, priority));

        // Filtrlar va statistikalar
        [HttpGet("completed")] public async Task<IActionResult> GetCompleted() => Ok(await _service.GetCompletedAsync());
        [HttpGet("pending")] public async Task<IActionResult> GetPending() => Ok(await _service.GetPendingAsync());
        [HttpGet("overdue")] public async Task<IActionResult> GetOverdue() => Ok(await _service.GetOverdueAsync());
        [HttpGet("search")] public async Task<IActionResult> Search(string query) => Ok(await _service.SearchAsync(query));
        [HttpGet("statistics")] public async Task<IActionResult> GetStats() => Ok(await _service.GetStatisticsAsync());
    }
}

