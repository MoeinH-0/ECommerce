using ShopManagement.Application.Contracts.Order;

namespace ShopManagement.Application;

public class CartService : ICartService
{
    private Cart Cart;

    public void Set(Cart cart)
    {
        Cart = cart;
    }

    public Cart Get()
    {
        return Cart;
    }
}