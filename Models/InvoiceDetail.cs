using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class InvoiceDetail : BaseModel
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; } = null!;
        public int InvoiceId { get; set; }

        [ValidateNever]
        public Invoice Invoice { get; set; } = null!;
    }
}
