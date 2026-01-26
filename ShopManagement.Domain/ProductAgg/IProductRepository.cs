using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Domain.ProductAgg;

public interface IProductRepository : IRepository<long, Product>
{
    EditProduct GetDetails(long id);
    List<ProductViewModel> Search(ProductSearchModel searchModel);
    List<ProductViewModel> GetProducts();
}