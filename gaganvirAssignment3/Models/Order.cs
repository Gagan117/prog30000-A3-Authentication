namespace gaganvirAssignment3.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
