namespace DreamDay.Entites;

public class BudgetItem
{
    public Guid Id { get; set; }
    public string Category { get; set; } = default!;
    public decimal AllocatedAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public string? Description { get; set; }
    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = default!;
}
