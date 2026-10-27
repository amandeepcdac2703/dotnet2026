using dotnet2026.Api;
using dotnet2026.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddApiVersioningConfiguration();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
