using System.ComponentModel.DataAnnotations;

namespace gaganvirAssignment3.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public Category Category { get; set; }
        [Range(100, 5000)]
        public decimal Price { get; set; }
        public bool Supports5G { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
