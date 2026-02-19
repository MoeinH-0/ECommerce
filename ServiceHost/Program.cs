using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using ServiceHost;
using ShopManagement.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var connectionString =
    builder.Configuration.GetConnectionString("ShopDatabase")!;

ShopManagementBootstrapper
    .Configuration(builder.Services, connectionString);

DiscountManagementBootstrapper
    .Configure(builder.Services, connectionString);

InventoryManagementBootstrapper
    .Configure(builder.Services, connectionString);

CommentManagementBootstrapper
    .Configure(builder.Services, connectionString);

BlogManagementBootstrapper
    .Configure(builder.Services, connectionString);

AccountManagementBootstrapper
    .Configure(builder.Services, connectionString);

builder.Services.AddSingleton
    (HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));

builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

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
app.MapRazorPages().WithStaticAssets();

app.MapDefaultControllerRoute();
app.Run();