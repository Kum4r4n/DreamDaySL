using DreamDay.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class GuestModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public MealPreference MealPreference { get; set; }

        [Required]
        public bool IsAttending { get; set; }

        public Guid WeddingId { get; set; }

        public IEnumerable<SelectListItem> MealOptions => new List<SelectListItem>
        {
            new SelectListItem { Text = "None", Value = ((int)MealPreference.NONE).ToString() },
            new SelectListItem { Text = "Vegetarian", Value = ((int)MealPreference.VEGETARIAN).ToString() },
            new SelectListItem { Text = "Non-Vegetarian", Value = ((int)MealPreference.NON_VEGETARIAN).ToString() },
            new SelectListItem { Text = "Vegan", Value = ((int)MealPreference.VEGAN).ToString() }
        };
    }
}
