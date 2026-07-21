using System.ComponentModel.DataAnnotations;

namespace ToDoLIST.Api.Entities
{
    public class UserRole
    {
        [Key]
        public long  RoleId { get; set; }
        public string Name { get; set; } = "User";
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
