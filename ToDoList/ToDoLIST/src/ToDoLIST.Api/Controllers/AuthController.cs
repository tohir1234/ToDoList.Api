using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoLIST.Api.Dtos;
using ToDoLIST.Api.Interfaces;

namespace ToDoLIST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto) => Ok(await _authService.RegisterAsync(dto));

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto) => Ok(await _authService.LoginAsync(dto));
        
        // Refresh va Logout funksiyalari (Service'da implementatsiya qilish kerak)
        [HttpPost("refresh-token")] 
        public IActionResult Refresh() => Ok(); 

        [HttpPost("logout")] 
        public IActionResult Logout() => Ok();
    }
}
