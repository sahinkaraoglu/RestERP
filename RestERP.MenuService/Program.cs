using Microsoft.EntityFrameworkCore;
using RestERP.MenuService.Data;
using RestERP.MenuService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabanı bağlantısı
builder.Services.AddDbContext<MenuDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MenuServiceConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
