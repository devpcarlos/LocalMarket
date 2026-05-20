
using Mapster;
using System.Reflection;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Repositories;
using LocalMarket.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using MapsterMapper;

namespace LocalMarket.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            // Supabase — lee variables de entorno
            var supaBaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL")
            ?? throw new InvalidOperationException("SUPABASE_URL no configurada en .env");


                var supaBaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY")
            ?? throw new InvalidOperationException("SUPABASE_KEY no configurada en .env");

            //supabase
            services.AddSingleton(sp =>
                {
                    var client = new Supabase.Client(supaBaseUrl, supaBaseKey,
                    new Supabase.SupabaseOptions
                    {
                        AutoRefreshToken = true,
                        AutoConnectRealtime = false
                });
                    client.InitializeAsync().GetAwaiter().GetResult();
                    return client;

                });
            // CONFIGURACIÓN DE MAPSTER

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            // Servicios y 
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();

            //Repositorios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            return services;
            }
        }
}
