namespace LTWeb_CodeFirst.Models
{
    public class Warranty : BaseModel
    {
        public string? Content { get; set; }   
        public ICollection<Car> Cars = new List<Car>(); 
    }
}
