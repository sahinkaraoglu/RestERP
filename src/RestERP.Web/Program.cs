using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Infrastructure.Context;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Anti-forgery token yapılandırması
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// HttpClient yapılandırması - API ile iletişim için
builder.Services.AddHttpClient("RestERPApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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

// Veritabanı ve Identity (migration/seed Web katmanından yönetilir)
builder.Services.AddDbContext<RestERPDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<RestERPDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Otomatik migration ve başlangıç seed verisi
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RestERPDbContext>();
    context.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { Role.Admin.ToString(), Role.Employee.ToString(), Role.Customer.ToString() };
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }

    var seedUsers = UserSeedData.GetUsers();
    var defaultPasswords = new Dictionary<string, string>
    {
        { "admin@resterp.com", "Admin123!" },
        { "employee@resterp.com", "Employee123!" },
        { "customer@test.com", "Customer123!" }
    };

    foreach (var seedUser in seedUsers)
    {
        var existingUser = await userManager.FindByEmailAsync(seedUser.Email);
        if (existingUser == null)
        {
            var password = defaultPasswords.GetValueOrDefault(seedUser.Email, "Password123!");
            var result = await userManager.CreateAsync(seedUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(seedUser, seedUser.RoleType.ToString());
            }
        }
        else if (!await userManager.IsInRoleAsync(existingUser, seedUser.RoleType.ToString()))
        {
            await userManager.AddToRoleAsync(existingUser, seedUser.RoleType.ToString());
        }
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

// Eski Admin/Menu URL'lerini yeni Food controller'a yönlendir (geri uyumluluk)
app.MapControllerRoute(
    name: "admin-menu-legacy",
    pattern: "Admin/Menu/{action=Index}/{id?}",
    defaults: new { area = "Admin", controller = "Food" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Panel}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
