using _0_Framework.Application;
using _01_ShopQuery.Contracts.Comment;
using _01_ShopQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly ShopContext _context;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;
    private readonly CommentContext _commentContext;

    public ProductQuery(ShopContext context, InventoryContext inventoryContext,
        DiscountContext discountContext, CommentContext commentContext)
    {
        _context = context;
        _discountContext = discountContext;
        _commentContext = commentContext;
        _inventoryContext = inventoryContext;
    }

    public ProductQueryModel GetDetails(string slug)
    {
        var inventory = _inventoryContext.Inventory
            .Select(x => new { x.ProductId, x.UnitPrice, x.InStock })
            .AsNoTracking().ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.UtcNow
                        && x.EndDate > DateTime.UtcNow)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate })
            .AsNoTracking().ToList();

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
            product.DoublePrice = price;
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

        product.Comments = _commentContext.Comments
            .Where(x => !x.IsCanceled)
            .Where(x => x.IsConfirmed)
            .Where(x => x.Type == CommentType.Product)
            .Where(x => x.OwnerRecordId == product.Id)
            .Select(x => new CommentQueryModel
            {
                Id = x.Id,
                Message = x.Message,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi()
            }).OrderByDescending(x => x.Id).ToList();

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
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).OrderByDescending(x => x.Id).Take(6).AsNoTracking().ToList();

        foreach (var product in products)
        {
            var productInventory =
                inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                product.DoublePrice = price;
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

    public List<ProductQueryModel> Search(string value)
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

        var query = _context.Products
            .Include(x => x.Category)
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                CategorySlug = x.Category.Slug,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShortDescription = x.ShortDescription,
                Slug = x.Slug
            }).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(value))
            query = query.Where(x => x.Name.Contains(value)
                                     || x.ShortDescription.Contains(value));

        var products = query.OrderByDescending(x => x.Id).ToList();

        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault
                (x => x.ProductId == product.Id);
            if (productInventory == null) continue;

            var price = productInventory.UnitPrice;
            product.Price = price.ToMoney();
            product.DoublePrice = price;
            var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
            if (discount == null) continue;

            var discountRate = discount.DiscountRate;
            product.DiscountRate = discountRate;
            product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
            product.HasDiscount = discountRate > 0;
            var discountAmount = Math.Round((price * discountRate) / 100);
            product.PriceWithDiscount = (price - discountAmount).ToMoney();
        }

        return products;
    }

    public List<CartItem> CheckInventoryStatus(List<CartItem> cartItems)
    {
        var inventories = _inventoryContext.Inventory.ToList();

        foreach (var cartItem in cartItems)
        {
            var inventory = inventories.FirstOrDefault(x =>
                x.ProductId == cartItem.Id && x.InStock);
            
            if (inventory != null)      
                cartItem.IsInStock = cartItem.Count <= inventory.CalculateCurrentCount();
        }

        return cartItems;
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