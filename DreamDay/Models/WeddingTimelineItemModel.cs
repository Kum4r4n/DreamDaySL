using DreamDay.Entites;

namespace DreamDay.Models
{
    public class WeddingTimelineItemModel
    {
        public Guid? Id { get; set; }
        public Guid WeddingId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime Time { get; set; }

    }
}
