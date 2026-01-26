using _0_Framework.Application;
using Microsoft.Extensions.Logging;

namespace ShopManagement.Application.Contracts.Product;

public interface IProductApplication
{
    OperationResult Create(CreateProduct command);
    OperationResult Edit(EditProduct command);
    OperationResult IsInStock(long id);
    OperationResult NotInStock(long id);
    EditProduct GetDetails(long id);
    List<ProductViewModel> Search(ProductSearchModel searchModel);
    List<ProductViewModel> GetProducts();
}