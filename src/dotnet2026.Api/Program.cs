
using dotnet2026.Api;
using dotnet2026.Api.Extensions;
using dotnet2026.Infrastructure.External;

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.RegisterServices(builder.Configuration);

// API versioning
builder.Services.AddApiVersioningConfiguration();

// Swagger
builder.Services.AddSwaggerConfiguration();

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Default HttpClientFactory
builder.Services.AddHttpClient();

// Named HttpClient
builder.Services.AddHttpClient("UniversityApi", (serviceProvider, client) =>
{
    var configuration = serviceProvider
        .GetRequiredService<IConfiguration>();

    var baseUrl = configuration[
        "ExternalApis:UniversityApi:BaseUrl"];

    if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
    {
        throw new InvalidOperationException(
            "University API BaseUrl is missing or invalid.");
    }

    client.BaseAddress = uri;
    client.Timeout = TimeSpan.FromSeconds(100);
});

// typed client

builder.Services.AddHttpClient<UniversityService>((serviceProvider, client) =>
{
    var configuration = serviceProvider
        .GetRequiredService<IConfiguration>();

    var baseUrl = configuration[
        "ExternalApis:UniversityApi:BaseUrl"];

    if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
    {
        throw new InvalidOperationException(
            "University API BaseUrl is missing or invalid.");
    }

    client.BaseAddress = uri;
    client.Timeout = TimeSpan.FromSeconds(100);
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
