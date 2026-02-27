using _01_ShopQuery.Contracts.Inventory;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore;

namespace _01_ShopQuery.Query;

public class InventoryQuery : IInventoryQuery
{
    private readonly InventoryContext _inventoryContext;
    private readonly ShopContext _shopContext;

    public InventoryQuery(InventoryContext inventoryContext,
        ShopContext shopContext)
    {
        _inventoryContext = inventoryContext;
        _shopContext = shopContext;
    }

    public StockStatus ChekStock(IsInStock command)
    {
        var productName = _shopContext.Products
            .Select(x => new { x.Id, x.Name })
            .FirstOrDefault(x => x.Id == command.ProductId)!
            .Name;

        var inventory = _inventoryContext.Inventory
            .FirstOrDefault(x => x.ProductId == command.ProductId);

        if (inventory == null || inventory.CalculateCurrentCount() >= command.Count)
            return new StockStatus
            {
                IsStock = false,
                ProductName = productName
            };


        return new StockStatus
        {
            IsStock = true,
        }; 
    }
}