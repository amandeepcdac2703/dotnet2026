using dotnet2026.Application.Interfaces;
using dotnet2026.Infrastructure.Persistence;
using dotnet2026.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register database connection factory
            services.AddSingleton<
                IDbConnectionFactory,
                SqlConnectionFactory>();

            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
