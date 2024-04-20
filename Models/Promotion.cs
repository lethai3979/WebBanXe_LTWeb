namespace LTWeb_CodeFirst.Models
{
    public class Promotion : BaseModel
    {
        public string? Content { get; set; }
        public decimal DiscountValue { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
