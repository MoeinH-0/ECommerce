using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using InventoryManagement.Presentation.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost;
using ShopManagement.Presentation.Api;
using Microsoft.AspNetCore.HttpOverrides;
using ShopManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

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
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
builder.Services.AddTransient<ISmsService, SmsService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults
        .AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/Account");
        o.LogoutPath = new PathString("/Account");
        o.AccessDeniedPath = new PathString("/AccessDenied");
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminArea", policy =>
        policy.RequireRole(new List<string> { Roles.Administrator, Roles.ContentUploader }))
    .AddPolicy("Shop", policy =>
        policy.RequireRole(new List<string> { Roles.Administrator }))
    .AddPolicy("Discount", policy =>
        policy.RequireRole(new List<string> { Roles.Administrator }))
    .AddPolicy("Account", policy =>
        policy.RequireRole(new List<string> { Roles.Administrator }));


builder.Services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
    .AddRazorPagesOptions(option =>
    {
        option.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
        option.Conventions.AuthorizeAreaFolder("Administration", "/Shop", "Shop");
        option.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discount");
        option.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
    })
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(InventoryController).Assembly)
    .AddNewtonsoftJson();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
    app.UseCors("DevCorsPolicy"); 
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.MapDefaultControllerRoute();

app.Run();