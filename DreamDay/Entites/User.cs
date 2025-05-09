using DreamDay.Enums;

namespace DreamDay.Entites;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!; 
    public string PasswordHash { get; set; } = default!;
    public Role Role { get; set; } = default!;

    public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();
}
