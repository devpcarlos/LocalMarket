
using Mapster;
using System.Reflection;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Repositories;
using LocalMarket.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using LocalMarket.Infrastructure.Persistence;
using LocalMarket.Infrastructure.Security;

namespace LocalMarket.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            // Supabase — lee variables de entorno
            var conection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("DB_CONNECTION_STRING no configurado");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(conection));
            // CONFIGURACIÓN DE MAPSTER

            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();

            // Servicios y 
            services.AddSingleton<IJwtService, JwtTokenProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IReviewService, ReviewService>();

            //Repositorios

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IBusinessCategoryRepository, BusinessCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
            }
        }
}
