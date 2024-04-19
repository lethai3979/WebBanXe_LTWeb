using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LTWeb_CodeFirst.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; } 
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<FavoriteList> FavoriteList { get; set;} = new List<FavoriteList>();
        public string? Image {  get; set; }
    }
}
