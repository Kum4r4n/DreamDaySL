using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class BudgetItemModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string? Description { get; set; }
        
        public Guid? VendorId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AllocatedAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SpentAmount { get; set; }

        public Guid WeddingId { get; set; }
    }
}
