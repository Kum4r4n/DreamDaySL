using DreamDay.Enums;

namespace DreamDay.Entites;

public class Guest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public MealPreference MealPreference { get; set; }

    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = default!;
}
