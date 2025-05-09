namespace DreamDay.Entites;

public class CheckListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
    public string? Description { get; set; }

    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = default!;
}
