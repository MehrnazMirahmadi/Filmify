using AutoMapper;
using Filmify.Application.Contracts;
using Filmify.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Filmify.Application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // --- Application Services
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IBoxService, BoxService>();

            // ✅ Register AutoMapper manually 
            services.AddSingleton<IMapper>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(ApplicationInstaller).Assembly);

                }, loggerFactory);

                return config.CreateMapper();
            });

            return services;
        }

    }
}
