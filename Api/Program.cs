using System.Text.Json;
using Api.Converters;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.Converters.Add(new ContentTypeConverter());
    options.JsonSerializerOptions.Converters.Add(new StatusTypeConverter());
});

builder.Services.AddOpenApi();

builder.Services.AddSingleton<JsonParserService>();
builder.Services.AddSingleton<CsvParserService>();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();