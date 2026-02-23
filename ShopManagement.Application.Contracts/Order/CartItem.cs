namespace ShopManagement.Application.Contracts.Order;

public class CartItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public double UnitPrice { get; set; }
    public int Count { get; set; }
    public bool IsInStock { get; set; }
    public int DiscountRate { get; set; }
    public double TotalPrice => UnitPrice * Count;
    public double DiscountAmount => TotalPrice * DiscountRate / 100;
    public double ItemPayAmount => TotalPrice - DiscountAmount;
}