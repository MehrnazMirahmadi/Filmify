using Filmify.Application.Contracts;
using Filmify.Domain.Contracts.Interfaces;
using Filmify.Infrastructure.Persistence.Context;
using Filmify.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Filmify.Infrastructure;

public static class InfrastructureInstaller
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- DbContext
        /*  services.AddDbContext<FilmifyDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));*/
        services.AddDbContext<FilmifyDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // برای نمایش تمام Query ها و اطلاعات حساس
            options.EnableSensitiveDataLogging();
            options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        });


        // --- Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IBoxRepository, BoxRepository>();
      
        // Add other repositories here (Box, Tag, etc.)

        // --- UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // --- Paging Service
        services.AddScoped<IPagingService, PagingService>();

        return services;
    }
}
