using _01_ShopQuery.Contracts.Inventory;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Presentation.Api;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryApplication _inventoryApplication;
    private readonly IInventoryQuery _inventoryQuery;
    
    public InventoryController(IInventoryApplication inventoryApplication,
        IInventoryQuery inventoryQuery)
    {
        _inventoryApplication = inventoryApplication;
        _inventoryQuery = inventoryQuery;
    }

    [HttpGet("{id:long}")]
    public List<InventoryOperationViewModel> GetOperationsBy([FromRoute]long id)
    {
        return _inventoryApplication.GetOperationLog(id);
    }
    
    [HttpPost]
    public StockStatus ChekStock([FromBody] IsInStock command)
    {
        return _inventoryQuery.ChekStock(command);
    }
}