using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IProductRepository _repository;

    public ProductApplication(IProductRepository repository)
    {
        _repository = repository;
    }

    public OperationResult Create(CreateProduct command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var product = new Product
        (command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify(), command.Code,
            command.ShortDescription, command.CategoryId);

        _repository.Create(product);

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public OperationResult Edit(EditProduct command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var product = _repository.Get(command.Id);
        if (product == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        product.Edit(command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify(), command.Code,
            command.ShortDescription, command.CategoryId);

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public EditProduct GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _repository.GetProducts();
    }
}