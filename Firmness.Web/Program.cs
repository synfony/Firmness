using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.Web.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using System.IO;
using Npgsql;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// Set EPPlus and QuestPDF licenses
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, 
        o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// --- Persist Data Protection Keys to the Database ---
builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>();
// ----------------------------------------------------

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Use ApplicationUser instead of IdentityUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Retry logic for database connection ---
int maxRetries = 10; // Increased retries for more resilience
int delayInSeconds = 5;
for (int i = 0; i < maxRetries; i++)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Attempting to connect to the database and apply migrations... (Attempt {AttemptNumber})", i + 1);
            dbContext.Database.Migrate();
            await SeedData.Initialize(services);
            logger.LogInformation("Database connection successful and migrations applied.");
            break; // Exit loop if successful
        }
    }
    catch (Exception ex) // Catching a more generic exception to handle SocketException and others
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Database connection failed. Retrying in {Delay} seconds...", delayInSeconds);
        if (i < maxRetries - 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
        }
        else
        {
            logger.LogError("Could not connect to the database after {MaxRetries} attempts. The application will now exit.", maxRetries);
            throw; // Re-throw the exception if all retries fail
        }
    }
}
// ---------------------------------

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This will serve files from wwwroot, including our 'recibos' directory

// --- Ensure the 'recibos' directory exists on startup ---
var wwwRootPath = app.Environment.WebRootPath;
if (wwwRootPath != null)
{
    var recibosPath = Path.Combine(wwwRootPath, "recibos");
    if (!Directory.Exists(recibosPath))
    {
        Directory.CreateDirectory(recibosPath);
    }
}
// ----------------------------------------------------

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
