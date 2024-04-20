using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class Invoice : BaseModel
    {
        public decimal Total { get; set; }
        public DateTime CreateOn { get; set; }
        public ICollection<InvoiceDetail> Invoices { get; set; } = new List<InvoiceDetail>();
        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; } = null!;
        public int? PromotionId { get; set; }
        [ValidateNever]
        public Promotion Promotion { get; set; } = null!;
    }
}
