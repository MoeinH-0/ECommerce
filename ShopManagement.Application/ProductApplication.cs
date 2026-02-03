using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductRepository _repository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    public ProductApplication(IProductRepository repository,
        IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
    {
        _repository = repository;
        _fileUploader = fileUploader;
        _productCategoryRepository = productCategoryRepository;
    }

    public OperationResult Create(CreateProduct command)
    {
        var operationResult = new OperationResult();
        if (_repository.Exists(x => x.Name == command.Name))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var categorySlug = _productCategoryRepository.GetSlugById(command.CategoryId);  
        var path = $"{categorySlug}//{command.Slug.Slugify()}";
        var picturePath = _fileUploader.Upload(command.Picture, path);
            
        var product = new Product
        (command.Name, command.Description, picturePath,
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

        var product = _repository.GetProductWithCategory(command.Id);
        if (product == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        
        var path = $"{product.Category.Slug}/{command.Slug.Slugify()}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        product.Edit(command.Name, command.Description, picturePath,
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