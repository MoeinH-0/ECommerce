using _01_ShopQuery.Contracts.Product;

namespace _01_ShopQuery.Contracts.ProductCategory;

public interface IProductCategoryQuery
{
    ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug);
    List<ProductCategoryQueryModel> GetProductCategories();
    List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();
    
}