using System.Text;
using Microsoft.EntityFrameworkCore;
using RestERP.Application.Services;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Interfaces;
using RestERP.Infrastructure;
using RestERP.Infrastructure.Repositories;
using RestERP.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabanı bağlantısı
builder.Services.AddDbContext<RestERPDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve UnitOfWork kayıtları
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servisleri kaydet
builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddTransient<IFoodService, FoodService>();
builder.Services.AddTransient<IFoodCategoryService, FoodCategoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
