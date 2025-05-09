namespace DreamDay.Entites;

public class Wedding
{
    public Guid Id { get; set; }
    public string PartnerOneName { get; set; } = default!;
    public string PartnerTwoName { get; set; } = default!;
    public DateTime WeddingDate { get; set; }
    public decimal TotalBudget { get; set; }


    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    public ICollection<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
    public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();
}
