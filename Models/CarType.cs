namespace LTWeb_CodeFirst.Models
{
    public class CarType : BaseModel
    {
        public string? Name { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<CarTypeDetail> CarTypeDetail { get; set; } = new List<CarTypeDetail>();   
    }
}
