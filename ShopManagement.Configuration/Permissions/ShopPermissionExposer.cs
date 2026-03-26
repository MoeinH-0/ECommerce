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
                    new(ShopPermission.ListProduct, "List Products"),
                    new(ShopPermission.SearchProduct, "Search Products"),
                    new(ShopPermission.CreateProduct, "Create Product"),
                    new(ShopPermission.EditProduct, "Edit Product")
                }
            },
            {
                "ProductCategory", new List<PermissionDto>
                {
                    new(ShopPermission.SearchProductCategory, "Search Product Categories"),
                    new(ShopPermission.ListProductCategory, "List Product Categories"),
                    new(ShopPermission.CreateProductCategory, "Create Product Category"),
                    new(ShopPermission.EditProductCategory, "Edit Product Category")
                }
            }
        };
    }
}