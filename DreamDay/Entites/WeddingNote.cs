namespace DreamDay.Entites
{
    public class WeddingNote
    {
        public Guid Id { get; set; }
        public Guid WeddingId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string SenderRole { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

    }
}
