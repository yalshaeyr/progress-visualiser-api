using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Contexts;
using ProgressVisualiserApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connection = string.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<MetricsContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddDbContext<MetricDataContext>(options =>
    options.UseSqlServer(connection));

app.RegisterMetricEndpoints();
app.RegisterMetricDataEndpoints();

app.Run();