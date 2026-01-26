using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductCategoryApplication : IProductCategoryApplication
{
    private readonly IProductCategoryRepository _repository;

    public ProductCategoryApplication(IProductCategoryRepository repository)
    {
        _repository = repository;
    }

    public OperationResult Create(CreateProductCategory command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var productCategory = new ProductCategory
        (command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify());

        _repository.Create(productCategory);

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public OperationResult Edit(EditProductCategory command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var productCategory = _repository.Get(command.Id);
        if (productCategory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        productCategory.Edit(command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify());

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public EditProductCategory GetDetails(long id)
    {
        return _repository.GetDetails(id)!;
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }

    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _repository.GetProductCategories();
    }
}