namespace LTWeb_CodeFirst.Models
{
    public class Company : BaseModel
    {
        public string? Name { get; set; }
        public ICollection<CarTypeDetail> CarTypeDetail { get; set; } = new List<CarTypeDetail>();
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
