using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class WeddingNoteModel
    {
        public Guid Id { get; set; }
        public Guid WeddingId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public string SenderRole { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
