using ABCRetail.Models;
using ABCRetail.Services;
using ABCRetailWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register storage services
builder.Services.AddSingleton<TableStorageService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddSingleton<QueueStorageService>();
builder.Services.AddSingleton<FileStorageService>();

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Seed default admin
using (var scope = app.Services.CreateScope())
{
    var tableService = scope.ServiceProvider.GetRequiredService<TableStorageService>();
    var hasher = new PasswordHasher<AdminEntity>();

    var existing = await tableService.GetAdminAsync("Admin", "admin"); // ? Fixed
    if (existing == null)
    {
        var admin = new AdminEntity
        {
            PartitionKey = "Admin",
            RowKey = "admin",
            Email = "admin@abcretail.com",
            FullName = "Admin Admin"
        };
        admin.PasswordHash = hasher.HashPassword(admin, "ChangeMe123!");
        await tableService.UpsertAdminAsync(admin);
        Console.WriteLine("Seeded default admin: admin@abcretail.com / ChangeMe123!");
    }
}

// Configure HTTP pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
