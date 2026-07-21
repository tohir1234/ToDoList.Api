using ToDoLIST.Api.Dtos;

namespace ToDoLIST.Api.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto registerdto);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto logindto);
    }
}
