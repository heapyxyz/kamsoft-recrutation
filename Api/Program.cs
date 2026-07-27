using System.Text.Json;
using Api.Models;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddOpenApi();

builder.Services.AddSingleton<JsonParserService>();
builder.Services.AddSingleton<CsvParserService>();

builder.Services.AddSingleton<Dictionary<ContentType, IContentParser>>(serviceProvider => new()
{
    [ContentType.Csv] = serviceProvider.GetRequiredService<CsvParserService>(),
    [ContentType.Json] = serviceProvider.GetRequiredService<JsonParserService>(),
});

var app = builder.Build();

// this is a recrutation task, so there's no need to run SwaggerUI only in development
// if this was a big project, I would use app.Environment.IsDevelopment()
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();