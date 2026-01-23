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

    public OperationResult Creat(CreatProductCategory command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            return operationResult.Failed("امکان ثبت رکورد تکراری وجود ندارد");

        var productCategory = new ProductCategory
        (command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify());

        _repository.Creat(productCategory);

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public OperationResult Edit(EditProductCategory command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operationResult.Failed("امکان ثبت رکورد تکراری وجود ندارد");

        var productCategory = _repository.Get(command.Id);
        if (productCategory == null)
            return operationResult.Failed("رکورد مورد نظر یافت نشد");

        productCategory.Edit(command.Name, command.Description, command.Picture,
            command.PictureAlt, command.PictureTitle, command.Keywords,
            command.MetaDescription, command.Slug.Slugify());

        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public EditProductCategory GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }
}