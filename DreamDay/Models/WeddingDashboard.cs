namespace DreamDay.Models
{
    public class WeddingDashboard
    {
        public Guid? WeddingId { get; set; }
        public Guid? SelectedWeddingId { get; set; }
        public string PartnerOneName { get; set; }
        public string PartnerTwoName { get; set; }
        public DateTime WeddingDate { get; set; }
        public decimal TotalBudget { get; set; }

        // To-do list for the couple's dashboard
        public List<ChecklistItemModel> ToDoList { get; set; }

        // You can extend this later with:
        // public int GuestCount { get; set; }
        // public decimal TotalSpent { get; set; }
        // public int TasksCompleted { get; set; }
    }
}
