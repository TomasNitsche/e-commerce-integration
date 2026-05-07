namespace ECommerce.Domain.Models;

public class Price
{
    public int Id { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime LastUpdated { get; set; }
}