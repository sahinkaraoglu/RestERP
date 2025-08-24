using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestERP.Application.Services;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
using RestERP.Infrastructure.Context;
using RestERP.Infrastructure.Repositories;
using RestERP.Web.Middleware;
using RestERP.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// Logging yapılandırması
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
    
    // Log seviyelerini yapılandır
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddFilter("Microsoft", LogLevel.Warning);
    logging.AddFilter("System", LogLevel.Warning);
    logging.AddFilter("RestERP", LogLevel.Information);
});

// Veritabanı bağlantısı
builder.Services.AddDbContext<RestERPDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT yapılandırması
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["JWT"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
        
    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));
});

// Repository ve UnitOfWork kayıtları
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servisleri kaydet
builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddTransient<IFoodService, FoodService>();
builder.Services.AddTransient<IFoodCategoryService, FoodCategoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReservationService, ReservationService>();
builder.Services.AddTransient<FoodCacheService>();

var app = builder.Build();

// Otomatik migration uygula
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RestERPDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Veritabanı migration işlemi başlatılıyor...");
        context.Database.Migrate();
        
        // Başarılı migration'ı log'a kaydet
        var log = new Log
        {
            Level = "Information",
            Message = "Veritabanı migration'ları başarıyla uygulandı.",
            Exception = null,
            StackTrace = null,
            Source = "Program.cs - Database Migration",
            UserId = "System",
            UserName = "System",
            RequestPath = "/",
            RequestMethod = "SYSTEM",
            IpAddress = "127.0.0.1",
            Timestamp = DateTime.UtcNow
        };
        
        context.Logs.Add(log);
        await context.SaveChangesAsync();
        
        logger.LogInformation("Veritabanı migration'ları başarıyla uygulandı.");
    }
    catch (Exception ex)
    {
        var context = services.GetRequiredService<RestERPDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        // Migration hatasını log'a kaydet
        var log = new Log
        {
            Level = "Error",
            Message = $"Migration sırasında hata oluştu: {ex.Message}",
            Exception = ex.ToString(),
            StackTrace = ex.StackTrace,
            Source = "Program.cs - Database Migration",
            UserId = "System",
            UserName = "System",
            RequestPath = "/",
            RequestMethod = "SYSTEM",
            IpAddress = "127.0.0.1",
            Timestamp = DateTime.UtcNow
        };
        
        try
        {
            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
        catch (Exception logEx)
        {
            logger.LogError(logEx, "Migration hatası log'a kaydedilirken hata oluştu");
        }
        
        logger.LogError(ex, "Migration sırasında hata oluştu: {Message}", ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Global logging middleware
app.UseMiddleware<LoggingMiddleware>();

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Panel}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
