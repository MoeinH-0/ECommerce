using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductRepository : RepositoryBase<long, Product>, IProductRepository
{
    private readonly ShopContext _context;

    public ProductRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditProduct GetDetails(long id)
    {
        return _context.Products.Select(x => new EditProduct
        {
            Id = x.Id,
            Name = x.Name,
            Code = x.Code,
            Description = x.Description,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            Slug = x.Slug,
            Picture = x.Picture,
            CreationDate = x.CreationDate.ToString("yyyy-MM-dd"),
            CategoryId = x.CategoryId,
            ShortDescription = x.ShortDescription
        }).FirstOrDefault(p => p.Id == id)!;
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel)
    {
        var products = _context.Products.Include(x => x.Category).ToList();

        var query = _context.Products
            .Include(x => x.Category)
            .Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToFarsi(),
                Category = x.Category.Name,
                CategoryId = x.CategoryId
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Code))
            query = query.Where(x => x.Code.Contains(searchModel.Code));

        if (searchModel.CategoryId != 0)
            query = query.Where(x => x.CategoryId == searchModel.CategoryId);


        return query.OrderByDescending(x => x.Id).ToList();
    }

    public List<ProductViewModel> GetProducts()
    {
        return _context.Products.Select(x => new ProductViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}