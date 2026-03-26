using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application;

public class InventoryApplication : IInventoryApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IInventoryRepository _repository;

    public InventoryApplication(IInventoryRepository repository, IAuthHelper authHelper)
    {
        _repository = repository;
        _authHelper = authHelper;
    }

    public OperationResult Create(CreateInventory command)
    {
        var operationResult = new OperationResult();

        if (_repository.Exists(x => x.ProductId == command.ProductId))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        _repository.Create(new Inventory(command.ProductId, command.UnitPrice));
        _repository.SaveChanges();
        return operationResult.Succeeded();
    }

    public OperationResult Edit(EditInventory command)
    {
        var operationResult = new OperationResult();

        var inventory = _repository.Get(command.Id);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (_repository.Exists(x => x.ProductId == command.ProductId
                                    && x.Id != command.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        inventory.Edit(command.ProductId, command.UnitPrice);
        _repository.SaveChanges();

        return operationResult.Succeeded();
    }

    public OperationResult Increase(IncreaseInventory command)
    {
        var operationResult = new OperationResult();

        var inventory = _repository.Get(command.InventoryId);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        inventory.Increase(command.Count, 1, command.Description);
        _repository.SaveChanges();

        return operationResult.Succeeded();
    }

    public OperationResult Reduce(ReduceInventory command)
    {
        var operationResult = new OperationResult();

        var inventory = _repository.Get(command.InventoryId);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _authHelper.CurrentAccountId();
        inventory.Reduce(command.Count, operatorId, command.Description, 0);
        _repository.SaveChanges();

        return operationResult.Succeeded();
    }

    public OperationResult Reduce(List<ReduceInventory> command)
    {
        var operationResult = new OperationResult();
        var operatorId = _authHelper.CurrentAccountId();

        foreach (var item in command)
        {
            var inventory = _repository.GetBy(item.ProductId);
            inventory!.Reduce(item.Count, operatorId, item.Description, item.OrderId);
        }

        _repository.SaveChanges();

        return operationResult.Succeeded();
    }

    public EditInventory GetDetails(long id)
    {
        return _repository.GetDetails(id);
    }

    public List<InventoryViewModel> Search(InventorySearchModel searchModel)
    {
        return _repository.Search(searchModel);
    }

    public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
    {
        return _repository.GetOperationLog(inventoryId);
    }
}