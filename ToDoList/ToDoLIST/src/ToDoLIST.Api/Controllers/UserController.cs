using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoLIST.Api.Dtos;
using ToDoLIST.Api.Interfaces;

namespace ToDoLIST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpPut("set-role")]
        public async Task<IActionResult> SetRole(long userId, string roleName) => Ok(await _userService.SetRoleAsync(userId, roleName));

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userService.GetAllUsersAsync());

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) => Ok(await _userService.DeleteUserAsync(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UserUpdateDto dto) => Ok(await _userService.UpdateUserAsync(id, dto));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id) => Ok(await _userService.GetUserByIdAsync(id));
    }
}
