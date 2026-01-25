using ShopManagement.Configuration;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Infrastructure.EFCore.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
ShopManagementBootstrapper.Configuration
    (builder.Services, builder.Configuration.GetConnectionString("ShopDatabase")!);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapDefaultControllerRoute();
app.Run();