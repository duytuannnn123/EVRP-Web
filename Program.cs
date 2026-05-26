using EVRP_Web.Data;
using EVRP_Web.Repositories;
using EVRP_Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));

    options.UseQueryTrackingBehavior(
        QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ALNSService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
