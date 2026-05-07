namespace ECommerce.Domain.Models;

public class Stock
{
    public int Id { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
}