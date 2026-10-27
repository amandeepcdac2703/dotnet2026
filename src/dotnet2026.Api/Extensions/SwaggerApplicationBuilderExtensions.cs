using Asp.Versioning.ApiExplorer;

namespace dotnet2026.Api.Extensions
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static WebApplication UseSwaggerConfiguration(
            this WebApplication app)
        {
            var provider =
                app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}
