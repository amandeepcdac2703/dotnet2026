using dotnet2026.Application;
using dotnet2026.Domain;
using dotnet2026.Infrastructure;

namespace dotnet2026.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices()
                .AddInfrastructureServices(configuration)
                .AddDomainServices();

            return services;
        }
    }
}
