using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);
var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
  DefaultRequestCulture = new RequestCulture(enUS),
  SupportedCultures = new List<CultureInfo> { enUS },
  SupportedUICultures = new List<CultureInfo> { enUS },
};

var connectionString = builder.Configuration.GetConnectionString("SalesWebMVCContext");
MySqlServerVersion serverVersion = new(ServerVersion.AutoDetect(connectionString));

builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(connectionString, serverVersion));

builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

// Seeding database
// Documentation: https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-6.0&tabs=visual-studio
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  SeedingService.Seed(services);
}

  // Configure the HTTP request pipeline.
  if (!app.Environment.IsDevelopment())
  {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
