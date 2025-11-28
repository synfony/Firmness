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

// --- Apply migrations and seed data on startup ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    await SeedData.Initialize(services);
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
