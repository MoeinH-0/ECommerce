using _0_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions;

public class ShopPermissionExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Product", new List<PermissionDto>
                {
                    new(10, "List Products"),
                    new(11, "Search Products"),
                    new(12, "Create Product"),
                    new(13, "Edit Product"),
                }
            },
            {
                "ProductCategory", new List<PermissionDto>
                {
                    new(20, "Search Product Categories"),
                    new(21, "List Product Categories"),
                    new(22, "Create Product Category"),
                    new(23, "Edit Product Category"),
                }
            }
        };
    }
}