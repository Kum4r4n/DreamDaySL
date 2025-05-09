using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class WeddingFormModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string PartnerOneName { get; set; }

        [Required]
        public string PartnerTwoName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime WeddingDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalBudget { get; set; }
    }
}
