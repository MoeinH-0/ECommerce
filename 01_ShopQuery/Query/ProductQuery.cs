using _0_Framework.Application;
using _01_ShopQuery.Contracts.Product;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly ShopContext _context;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;

    public ProductQuery(ShopContext context, InventoryContext inventoryContext, DiscountContext discountContext)
    {
        _context = context;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public ProductQueryModel GetDetails(string slug)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice, x.InStock }).AsNoTracking().ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).AsNoTracking().ToList();

        var product = _context.Products
            .Include(x => x.Category)
            .Include(x => x.ProductPictures)
            .AsNoTracking()
            .Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Name,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug,
                CategorySlug = product.Category.Slug,
                Code = product.Code,
                Description = product.Description,
                Keywords = product.Keywords,
                MetaDescription = product.MetaDescription,
                ShortDescription = product.ShortDescription,
                Pictures = MapProductPictures(product.ProductPictures)
            }).FirstOrDefault(x => x.Slug == slug);

        if (product == null)
            return new ProductQueryModel();

        var productInventory =
            inventory.FirstOrDefault(x => x.ProductId == product.Id);
        if (productInventory != null)
        {
            product.IsInStock = productInventory.InStock;
            var price = productInventory.UnitPrice;
            product.Price = price.ToMoney();
            var discount =
                discounts.FirstOrDefault(x => x.ProductId == product.Id);

            if (discount != null)
            {
                product.DiscountRate = discount.DiscountRate;
                product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                product.HasDiscount = true;
                product.PriceWithDiscount = Math.Round(price * (100 - product.DiscountRate) / 100).ToMoney();
            }
        }

        return product;
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow
                        && x.EndDate > DateTime.UtcNow)
            .Select(x => new { x.DiscountRate, x.ProductId }).AsNoTracking().ToList();

        var products = _context.Products
            .Include(x => x.Category)
            .Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Name,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug
            }).OrderByDescending(x => x.Id).Take(6).AsNoTracking().ToList();

        foreach (var product in products)
        {
            var productInventory =
                inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount =
                    discounts.FirstOrDefault(x => x.ProductId == product.Id);

                if (discount != null)
                {
                    product.DiscountRate = discount.DiscountRate;
                    product.HasDiscount = true;
                    product.PriceWithDiscount =
                        Math.Round(price * (100 - product.DiscountRate) / 100).ToMoney();
                }
            }
        }

        return products;
    }

    private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
    {
        return pictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemoved,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemoved)
            .ToList();
    }
}