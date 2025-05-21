using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ocelot JSON konfigürasyonu için
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ocelot servisini ekleme
builder.Services.AddOcelot(builder.Configuration);

// CORS için
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// CORS middleware
app.UseCors("CorsPolicy");

// Ocelot middleware
await app.UseOcelot();

app.Run();
