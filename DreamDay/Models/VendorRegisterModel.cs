using System.ComponentModel.DataAnnotations;
using DreamDay.Enums;

namespace DreamDay.Models;

public class VendorRegisterModel
{
    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Required, DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string Category { get; set; }
    public string? Description { get; set; }
    public string? ContactInfo { get; set; }
    public decimal PriceEstimate { get; set; }
}
