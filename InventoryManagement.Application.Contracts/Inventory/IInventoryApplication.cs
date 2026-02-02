using _0_Framework.Application;

namespace InventoryManagement.Application.Contracts.Inventory;

public interface IInventoryApplication
{
    OperationResult Create(CreateInventory command);
    OperationResult Edit(EditInventory command);
    OperationResult Increase(IncreaseInventory command);
    OperationResult Reduce(List<DecreaseInventory> command);
    OperationResult Reduce(DecreaseInventory command);
    EditInventory GetDetails(long id);
    List<InventoryViewModel> Search(InventorySearchModel searchModel);
    List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
}