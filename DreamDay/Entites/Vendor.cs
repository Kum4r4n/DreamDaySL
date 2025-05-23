﻿namespace DreamDay.Entites
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ContactInfo { get; set; }
        public decimal PriceEstimate { get; set; }
    }
}
