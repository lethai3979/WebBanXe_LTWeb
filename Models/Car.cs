using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LTWeb_CodeFirst.Models
{
    public class Car : BaseModel 
    {
        public string? Name { get; set; }
        public int Seat { get; set; }
        public string? CarImages { get; set;}
        [ValidateNever]
        public bool Gear { get; set; } = true;
        public decimal Price { get; set; }
        public int CarTypeId { get; set; }
        [ValidateNever]
        public CarType CarType { get; set; } = null!;
        public int CompanyId { get; set; }
        [ValidateNever]
        public Company Company { get; set; } = null!;
        public int WarrantyId { get; set; }
        [ValidateNever]
        public Warranty Warranty { get; set; } = null!;
        public ICollection<InvoiceDetail> Invoices { get; set; } = new List<InvoiceDetail>();
        public ICollection<FavoriteList> FavoriteList { get; set; } = new List<FavoriteList>(); 
    }
}
