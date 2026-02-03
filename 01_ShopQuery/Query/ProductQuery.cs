using _0_Framework.Application;
using _01_ShopQuery.Contracts.Product;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly ShopContext _context;
    private readonly InventoryContext _inventoryContext;
    private readonly DiscountContext _discountContext;

    public ProductQuery(ShopContext context, InventoryContext inventoryContext, DiscountContext discountContext)
    {
        _context = context;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();
        
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
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
            var productInventory = inventory.
                FirstOrDefault(x => x.ProductId == product.Id);
            
            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.
                    FirstOrDefault(x => x.ProductId == product.Id);
                
                if (discount != null)
                {
                    product.DiscountRate = discount.DiscountRate;
                    product.HasDiscount = true;
                    product.PriceWithDiscount = Math.Round(price * (100 - product.DiscountRate) / 100).ToMoney();
                }
                else
                    product.HasDiscount = false;
            }
        }

        return products;
    }
}