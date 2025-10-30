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

// Otomatik migration - veritabanını otomatik güncelle
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RestERPDbContext>();
    context.Database.Migrate();
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
