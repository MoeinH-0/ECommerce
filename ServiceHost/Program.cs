using _0_Framework.Application;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using ServiceHost;
using ShopManagement.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

ShopManagementBootstrapper.Configuration
    (builder.Services, builder.Configuration.GetConnectionString("ShopDatabase")!);

DiscountManagementBootstrapper.Configure
    (builder.Services, builder.Configuration.GetConnectionString("ShopDatabase")!);

InventoryManagementBootstrapper.Configure
    (builder.Services, builder.Configuration.GetConnectionString("ShopDatabase")!);

builder.Services.AddTransient<IFileUploader, FileUploader>();

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