namespace ShopManagement.Application.Contracts.Order;

public class Cart
{
    public Cart()
    {
        Items = [];
    }

    public double TotalAmount => Items.Sum(x => x.TotalPrice);
    public double DiscountAmount => Items.Sum(x => x.DiscountAmount);
    public double PayAmount => Items.Sum(x => x.ItemPayAmount);
    public List<CartItem> Items { get; set; }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
    }
}