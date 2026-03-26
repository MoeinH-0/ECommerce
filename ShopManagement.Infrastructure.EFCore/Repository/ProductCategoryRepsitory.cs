using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductCategoryRepository : RepositoryBase<long, ProductCategory>, IProductCategoryRepository
{
    private readonly ShopContext _context;

    public ProductCategoryRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditProductCategory? GetDetails(long id)
    {
        return _context.ProductCategories.Select(x => new EditProductCategory
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Slug = x.Slug,
            // Picture = x.Picture,
            CreationDate = x.CreationDate.ToString("yyyy-MM-dd")
        }).FirstOrDefault(p => p.Id == id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        var query = _context.ProductCategories.Select(x => new ProductCategoryViewModel
        {
            Id = x.Id,
            Picture = x.Picture,
            Name = x.Name,
            CreationDate = x.CreationDate.ToString("yyyy-MM-dd"),
            ProductsCount = _context.ProductCategories.Count()
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        return query.OrderByDescending(x => x.Id).ToList();
    }

    public string GetSlugById(long id)
    {
        return _context.ProductCategories
            .Select(x => new { x.Id, x.Slug }).FirstOrDefault(x => x.Id == id)!.Slug;
    }

    public List<ProductCategoryViewModel> GetProductCategories()
    {
        return _context.ProductCategories
            .Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
    }
}