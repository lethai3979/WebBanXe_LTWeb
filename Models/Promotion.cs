namespace LTWeb_CodeFirst.Models
{
    public class Promotion : BaseModel
    {
        public decimal DiscountValue { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
