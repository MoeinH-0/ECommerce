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
                    new(50, "List Inventory"),
                    new(51, "Search Inventory"),
                    new(52, "Create Inventory"),
                    new(53, "Edit Inventory"),
                }
            }
        };
    }
}