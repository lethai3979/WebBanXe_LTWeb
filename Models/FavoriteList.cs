using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class FavoriteList : BaseModel
    {
        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; } = null!;
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
    }
}
