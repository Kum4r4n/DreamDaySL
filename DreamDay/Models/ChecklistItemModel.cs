using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class ChecklistItemModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DueDate { get; set; }
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public int WeddingId { get; set; }
    }
}
