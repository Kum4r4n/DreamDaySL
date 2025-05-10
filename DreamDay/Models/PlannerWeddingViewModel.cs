namespace DreamDay.Models
{
    public class PlannerWeddingViewModel
    {
        public Guid? Id { get; set; }
        public string PartnerOneName { get; set; } = string.Empty;
        public string PartnerTwoName { get; set; } = string.Empty;
        public DateTime WeddingDate { get; set; }
        public decimal TotalBudget { get; set; }
    }
}
