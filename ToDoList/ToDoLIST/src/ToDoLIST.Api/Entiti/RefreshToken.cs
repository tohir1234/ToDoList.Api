namespace ToDoLIST.Api.Entiti
{
    public class RefreshToken
    {

        public long Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public long UserId { get; set; }
    }
}
