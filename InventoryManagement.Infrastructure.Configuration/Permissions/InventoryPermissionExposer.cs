using _0_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permissions;

public class InventoryPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Inventory", new List<PermissionDto>
                {
                    new(InventoryPermissions.ListInventory, "List Inventory"),
                    new(InventoryPermissions.SearchInventory, "Search Inventory"),
                    new(InventoryPermissions.CreateInventory, "Create Inventory"),
                    new(InventoryPermissions.EditInventory, "Edit Inventory"),
                    new(InventoryPermissions.IncreaseInventory, "Increase Inventory"),
                    new(InventoryPermissions.ReduceInventory, "Reduce Inventory"),
                    new(InventoryPermissions.OperationLog, "Operation Log")
                }
            }
        };
    }
}