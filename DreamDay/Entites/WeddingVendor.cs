namespace DreamDay.Entites
{
    public class WeddingVendor
    {
        public Guid Id { get; set; }

        public Guid WeddingId { get; set; }
        public Wedding Wedding { get; set; } = default!;

        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; } = default!;
    }
}
