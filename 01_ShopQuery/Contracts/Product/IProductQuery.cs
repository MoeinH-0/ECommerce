using ShopManagement.Application.Contracts.Order;

namespace _01_ShopQuery.Contracts.Product;

public interface IProductQuery
{
    ProductQueryModel GetDetails(string slug);
    List<ProductQueryModel> GetLatestArrivals();
    List<ProductQueryModel> Search (string value);
    List<CartItem> CheckInventoryStatus(List<CartItem> cartItems);
}