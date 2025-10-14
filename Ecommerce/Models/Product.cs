namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Category { get; set; }
        
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; } 
        public DateTime EditedDate { get; set; }
        public int EditedBy { get; set; }
        
        
    }
}
