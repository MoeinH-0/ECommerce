namespace _01_ShopQuery.Contracts.Product;

public interface IProductQuery
{
    ProductQueryModel GetDetails(string slug);
    List<ProductQueryModel> GetLatestArrivals();
    
}