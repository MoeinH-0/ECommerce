using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Infrastructure.Configuration.Permissions;
using ShopPermission = ShopManagement.Infrastructure.Configuration.Permissions.ShopPermission;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductCategories;

//[Authorize(Roles = "1, 3")]
public class IndexModel : PageModel
{
    private readonly IProductCategoryApplication _productCategoryApplication;
    public List<ProductCategoryViewModel> ProductCategories;
    public ProductCategorySearchModel SearchModel;

    public IndexModel(IProductCategoryApplication productCategoryApplication)
    {
        _productCategoryApplication = productCategoryApplication;
    }

    [NeedsPermission(ShopPermission.ListProductCategory)]
    public void OnGet(ProductCategorySearchModel searchModel)
    {
        ProductCategories = _productCategoryApplication.Search(searchModel);
    }

    public IActionResult OnGetCreate()
    {
        return Partial("./Create", new CreateProductCategory());
    }

    [NeedsPermission(ShopPermission.CreateProductCategory)]
    public JsonResult OnPostCreate(CreateProductCategory command)
    {
        var result = _productCategoryApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var productCategory = _productCategoryApplication.GetDetails(id);
        return Partial("Edit", productCategory);
    }

    [NeedsPermission(ShopPermission.EditProductCategory)]
    public JsonResult OnPostEdit(EditProductCategory command)
    {
        var result = _productCategoryApplication.Edit(command);
        return new JsonResult(result);
    }
}