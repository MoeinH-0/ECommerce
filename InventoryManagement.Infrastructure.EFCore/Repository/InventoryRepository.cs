using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository;

public class InventoryRepository : RepositoryBase<long, Inventory>, IInventoryRepository
{
    private readonly ShopContext _shopContext;
    private readonly InventoryContext _inventoryContext;
    private readonly AccountContext _accountContext;
    public InventoryRepository(InventoryContext context,
        ShopContext shopContext, AccountContext accountContext) : base(context)
    {
        _inventoryContext = context;
        _shopContext = shopContext;
        _accountContext = accountContext;
    }

    public EditInventory? GetDetails(long id)
    {
        return _inventoryContext.Inventory.Select(x => new EditInventory
        {
            Id = x.Id,
            ProductId = x.ProductId,
            UnitPrice = x.UnitPrice
        }).FirstOrDefault(x => x.Id == id);
    }

    public Inventory? GetBy(long productId)
    {
        return _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == productId);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        var product = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();
        
        var query = _inventoryContext.Inventory.Select(x => new InventoryViewModel
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
        var accounts = _accountContext.Accounts
            .Select(x => new { x.Id, x.FullName })
            .ToList();

        var inventory = _inventoryContext.Inventory
            .FirstOrDefault(x => x.Id == inventoryId)!;
        
        var operations =inventory.Operations
            .Select(x => new InventoryOperationViewModel
            {
                CurrentCount = x.CurrentCount,
                Count = x.Count,
                Description = x.Description,
                Id = x.Id,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                OperatorId = x.OperatorId,
                OrderId = x.OrderId
            }).OrderByDescending(x => x.Id).ToList();
        
        foreach(var operation in operations)
            operation.Operator = accounts.
                FirstOrDefault(x => x.Id == operation.OperatorId)?.FullName;
        
        return operations;
    }
}