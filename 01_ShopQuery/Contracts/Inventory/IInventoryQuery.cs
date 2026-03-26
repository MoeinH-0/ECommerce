namespace _01_ShopQuery.Contracts.Inventory;

public interface IInventoryQuery
{
    StockStatus ChekStock(IsInStock command);
}