using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<ProgressVisualiserApiContext>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();

app.RegisterMetricController();
app.RegisterMetricDataController();

app.Run();

// expose Program class to ProgressVisualiserApiWebApplicationFactory
// see https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }