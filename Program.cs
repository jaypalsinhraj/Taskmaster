using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using TaskMaster.Data;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

    // Use DefaultAzureCredential - works with Managed Identity in Azure AND az login locally
    builder.Configuration.AddAzureKeyVault(
        keyVaultUri,
        new DefaultAzureCredential()
    );
}

// Add services
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    context.Database.EnsureCreated();
}

app.Run();