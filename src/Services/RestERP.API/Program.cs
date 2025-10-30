using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestERP.Application.Services;
using RestERP.Application.Services.Abstract;
using RestERP.Application.Services.Concrete;
using RestERP.Core.Interfaces;
using RestERP.Infrastructure.Context;
using RestERP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Infrastructure.Data.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Autofac entegrasyonu
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Repository ve UnitOfWork
    containerBuilder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>))
                   .InstancePerLifetimeScope();

    containerBuilder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

    containerBuilder.RegisterModule(new RestERP.API.DependencyInjection.AutofacModule());
});

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI yapılandırması
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "RestERP API", 
        Version = "v1",
        Description = "RestERP Restaurant Management System API"
    });
    
    // JWT Authentication için Swagger yapılandırması
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// CORS yapılandırması
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// HttpContextAccessor kaydı
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

// Identity kaydı
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
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
        
    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));
});


var app = builder.Build();

// Otomatik migration - veritabanını otomatik güncelle ve başlangıç kullanıcı/rol seed'i
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RestERPDbContext>();
    context.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Rolleri oluştur
    var roles = new[] { Role.Admin.ToString(), Role.Employee.ToString(), Role.Customer.ToString() };
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }

    // UserSeedData'dan kullanıcıları oluştur
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
        else
        {
            // Rol atamasını garanti et
            if (!await userManager.IsInRoleAsync(existingUser, seedUser.RoleType.ToString()))
            {
                await userManager.AddToRoleAsync(existingUser, seedUser.RoleType.ToString());
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestERP API v1");
        c.RoutePrefix = string.Empty; // Swagger UI'ı root'ta göster
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
