using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class CarTypeDetail : BaseModel
    {
        public int CompanyId { get; set; }
        [ValidateNever]
        public Company Company { get; set; } = null!;
        public int CarTypeId { get; set; }
        [ValidateNever]
        public CarType CarType { get; set; } = null!;
    }
}
