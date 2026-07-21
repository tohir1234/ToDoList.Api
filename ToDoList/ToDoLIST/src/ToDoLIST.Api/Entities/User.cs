namespace ToDoLIST.Api.Entities;

public class User
{
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public long RoleId { get; set; }
    public UserRole Role { get; set; } = null!;
    public RefreshToken? RefreshToken { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
}
