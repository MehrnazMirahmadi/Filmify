using Filmify.Api.Middleware;
using Filmify.Application;
using Filmify.Infrastructure;
using Filmify.Infrastructure.Persistence.Context;
using Filmify.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- Configure Serilog ---
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/filmify-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// --- Services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Filmify API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<FilmifyDbContext>("database");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7239")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Filmify API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// --- Seed DB ---
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FilmifyDbContext>();
        Log.Information("Starting database migration...");
        await db.Database.MigrateAsync();
        Log.Information("Database migration completed successfully");

        Log.Information("Starting database seeding...");
        await DbInitializer.SeedAsync(db);
        Log.Information("Database seeding completed successfully");
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred during database initialization");
    throw;
}

try
{
    Log.Information("Starting Filmify API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
