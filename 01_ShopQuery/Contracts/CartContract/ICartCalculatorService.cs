using ShopManagement.Application.Contracts.Order;

namespace _01_ShopQuery.Contracts.CartContract;

public interface ICartCalculatorService
{
    public Cart ComputeCart(List<CartItem> cartItems);
}