using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly InventoryContext _context;
    private readonly ShopContext _shopContext;

    public InventoryRepository(InventoryContext context) : base(context)
    {
        _context = context;
    }

    public EditInventory? GetDetails(long id)
    {
        return _context.Inventory.Select(x => new EditInventory
        {
            Id = x.Id,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).FirstOrDefault(x => x.Id == id);
    }

    public Inventory? GetBy(long productId)
    {
        return _context.Inventory.FirstOrDefault(x => x.ProductId == productId);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        var product = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        
        var query = _context.Inventory.Select(x => new InventoryViewModel
        {
            Id = x.Id,
            UnitPrice = x.UnitPrice,
            InStock = x.InStock,
            ProductId = x.ProductId,
            CurrentCount = x.CalculateCurrentCount()
        });
        
        if (searchModel.ProductId > 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        if (searchModel.InStock)
            query = query.Where(x => !x.InStock);
        
        var inventory = query.OrderByDescending(x => x.Id).ToList();
        
        inventory.ForEach(item =>
        {
            item.Product = product.FirstOrDefault(x => x.Id == item.ProductId)?.Name;
        });
        
        return inventory;
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        return _context.Inventory.FirstOrDefault(x => x.Id == inventoryId)
            .Operations.Select(x => new InventoryOperationViewModel
            {
                CurrentCount = x.CurrentCount,
                Count = x.Count,
                Description = x.Description,
                Id = x.Id,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                Operator = "مدیر سیستم",
                OperatorId = x.OperatorId,
                OrderId = x.OrderId
            }).OrderByDescending(x => x.Id).ToList();
    }
}