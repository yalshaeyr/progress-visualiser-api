using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Controllers;
using ProgressVisualiserApi.Services;

var AllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                [
                    "http://localhost:5173",
                    "https://red-tree-0360f6300.6.azurestaticapps.net"
                ]
            )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var sqlConnectionString = string.Empty;
var appInsightsConnectionString = string.Empty;

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    sqlConnectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
    appInsightsConnectionString = builder.Configuration.GetConnectionString("AZURE_APPLICATIONINSIGHTS_CONNECTIONSTRING");
}

else
{
    sqlConnectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
    appInsightsConnectionString = Environment.GetEnvironmentVariable("AZURE_APPLICATIONINSIGHTS_CONNECTIONSTRING");
}

builder.Services.AddOpenTelemetry().UseAzureMonitor(options => {
    options.ConnectionString = appInsightsConnectionString;
});

builder.Services.AddDbContext<ProgressVisualiserApiContext>(options =>
    options.UseSqlServer(
        sqlConnectionString,
        // specifically tailored for SQL Server, including Azure SQL
        // see https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
        // since we plan to use a free Azure SQL database deployment,
        // which requires warmup, this will address transient failures during this period
        options => options.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null
        )
    )
);

// We don't need more than one instance of this service in the application lifetime
builder.Services.AddSingleton<IContentSafetyService, ContentSafetyService>();

var app = builder.Build();

app.RegisterMetricController();
app.RegisterMetricDataController();

app.UseCors(AllowSpecificOrigins);

app.Run();

// expose Program class to ProgressVisualiserApiWebApplicationFactory
// see https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }