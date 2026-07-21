namespace ToDoLIST.Api.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public long RefreshTokenId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresAt;
        public long UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
