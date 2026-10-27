using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace dotnet2026.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(
            this IServiceCollection services)
        {
            services.AddTransient<
                IConfigureOptions<SwaggerGenOptions>,
                ConfigureSwaggerOptions>();

            services.AddSwaggerGen();

            return services;
        }
    }
}
