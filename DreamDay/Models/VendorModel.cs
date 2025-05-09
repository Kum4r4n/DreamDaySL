namespace DreamDay.Models
{
    public class VendorModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string? ContactInfo { get; set; }
        public decimal PriceEstimate { get; set; }
    }
}
