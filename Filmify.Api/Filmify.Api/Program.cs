using Filmify.Application;
using Filmify.Infrastructure;
using Filmify.Infrastructure.Persistence.Context;
using Filmify.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// --- Services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(); 

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

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
app.UseAuthorization();

app.MapControllers();

// --- Seed DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FilmifyDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(db);
}

app.Run();
