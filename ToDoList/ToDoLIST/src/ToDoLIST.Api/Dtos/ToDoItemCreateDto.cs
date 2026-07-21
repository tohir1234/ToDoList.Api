namespace ToDoLIST.Api.Dtos;

public class ToDoItemCreateDto
{
    public string Titile { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }

}
