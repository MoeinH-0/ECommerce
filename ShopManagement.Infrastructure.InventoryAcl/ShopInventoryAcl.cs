using InventoryManagement.Application.Contracts.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl;

public class ShopInventoryAcl : IShopInventoryAcl
{
    private readonly IInventoryApplication _inventoryApplication;

    public ShopInventoryAcl(IInventoryApplication inventoryApplication)
    {
        _inventoryApplication = inventoryApplication;
    }

    public bool ReduceFromInventory(List<OrderItem> items)
    {
        var list = items.Select(item =>
            new ReduceInventory(item.ProductId, item.Count,
                "خرید مشتری", item.OrderId)).ToList();

        return _inventoryApplication.Reduce(list).IsSucceeded;
    }
}