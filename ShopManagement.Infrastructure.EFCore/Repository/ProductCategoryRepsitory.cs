using System.Linq.Expressions;
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
            Picture = x.Picture,
            CreationDate = x.CreateDate.ToString("yyyy-MM-dd"),
        }).FirstOrDefault(p => p.Id == id);
    }

    public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
    {
        return _context.ProductCategories.Select(x => new ProductCategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                CreationDate = x.CreateDate.ToString("yyyy-MM-dd"),
                ProductsCount = _context.ProductCategories.Count()
            }).Where(x => !string.IsNullOrWhiteSpace(x.Name)
                          && x.Name.Contains(searchModel.Name))
            .OrderByDescending(x => x.Id)
            .ToList();
    }
}