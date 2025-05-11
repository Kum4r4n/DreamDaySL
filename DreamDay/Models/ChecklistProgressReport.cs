namespace DreamDay.Models
{
    public class ChecklistProgressReport
    {
        public Guid WeddingId { get; set; }
        public string CoupleName { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Completed { get; set; }
        public int PercentComplete { get; set; }
    }
}
