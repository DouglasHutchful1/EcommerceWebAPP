namespace Ecommerce.Models;

public class Vendor
{
    public int id { get; set; }
    
    public string Name { get; set; }
    public int UserIdfk { get; set; }
    
    public string? Description { get; set; }
    
    public bool Active { get; set; }
    
    public bool? VendorDeliveryQuality { get; set; }
    
    public DateTime? CreationDate { get; set; }
    public DateTime? EditedDate { get; set; }
    
}