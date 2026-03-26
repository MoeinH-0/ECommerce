using _0_Framework.Domain;

namespace InventoryManagement.Domain.InventoryAgg;

public class Inventory : EntityBase
{
    public Inventory(long productId, double unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        InStock = false;
    }

    public long ProductId { get; private set; }
    public double UnitPrice { get; private set; }
    public bool InStock { get; private set; }
    public List<InventoryOperation> Operations { get; private set; }

    public void Edit(long productId, double unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
    }

    public long CalculateCurrentCount()
    {
        return Operations.Last().CurrentCount;
    }

    public void Increase(long count, long operatorId, string description)
    {
        var operation = new InventoryOperation(true, count, operatorId,
            CalculateCurrentCount(), description, 0, Id);
        Operations.Add(operation);
        UpdateInStock();
    }

    public void Reduce(long count, long operatorId, string description, long orderId)
    {
        var operation = new InventoryOperation(false, count, operatorId,
            CalculateCurrentCount(), description, orderId, Id);
        Operations.Add(operation);
        UpdateInStock();
    }

    private void UpdateInStock()
    {
        InStock = CalculateCurrentCount() > 0;
    }
}