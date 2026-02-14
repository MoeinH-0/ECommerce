using _0_Framework.Application;
using _01_ShopQuery.Contracts.Product;
using _01_ShopQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ShopContext _shopContext;
    private readonly InventoryContext _inventoryContext;
    private readonly DiscountContext _discountContext;

    public ProductCategoryQuery(ShopContext shopContext, InventoryContext inventoryContext,
        DiscountContext discountContext)
    {
        _shopContext = shopContext;
        _inventoryContext = inventoryContext;
        _discountContext = discountContext;
    }

    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _shopContext.ProductCategories
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).AsNoTracking().ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow)
            .Select(x => new { x.ProductId, x.DiscountRate })
            .ToList();

        var categories = _shopContext.ProductCategories
            .Include(x => x.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                Products = MapProducts(x.Products)
            }).AsNoTracking().ToList();

        foreach (var category in categories)
        {
            foreach (var product in category.Products)
            {
                var productInventory = inventory.FirstOrDefault
                    (x => x.ProductId == product.Id);

                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();

                    var discount = discounts.FirstOrDefault
                        (x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        product.DiscountRate = discount.DiscountRate;
                        product.HasDiscount = true;
                        product.PriceWithDiscount = Math.Round(price * (100 - product.DiscountRate) / 100).ToMoney();
                    }
                }
            }
        }

        return categories;
    }

    private static List<ProductQueryModel> MapProducts(List<Product> products)
    {
        return products.Select(x => new ProductQueryModel
        {
            Id = x.Id,
            Category = x.Category.Name,
            Name = x.Name,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Slug = x.Slug
        }).OrderByDescending(x => x.Id).ToList();
    }

    public ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).AsNoTracking().ToList();

        var category = _shopContext.ProductCategories
            .Include(a => a.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                MetaDescription = x.MetaDescription,
                Keywords = x.Keywords,
                Slug = x.Slug,
                Products = MapProducts(x.Products)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        foreach (var product in category.Products)
        {
            var productInventory = inventory
                .FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.
                    FirstOrDefault(x => x.ProductId == product.Id);

                if (discount != null)
                {
                    product.DiscountRate = discount.DiscountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = true;
                    product.PriceWithDiscount = Math.Round(price * (100 - product.DiscountRate) / 100).ToMoney();
                }
            }
        }

        return category;
    }
}