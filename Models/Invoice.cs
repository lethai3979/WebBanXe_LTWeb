using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class Invoice : BaseModel
    {
        public decimal Total { get; set; }
        public DateTime CreateOn { get; set; }
        public ICollection<InvoiceDetail> Invoices { get; set; } = new List<InvoiceDetail>();
        public int PromotionId { get; set; }
        [ValidateNever]
        public Promotion Promotion { get; set; } = null!;

        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
    }
}
