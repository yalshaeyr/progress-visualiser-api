using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Tests.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ProgressVisualiserApi.Tests;
    
public class ProgressVisualiserApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's DbContext registration
            // So the app does not try to connect to a real Azure SQL database
            // In .NET 8, removing a typeof(DbContextOptions) will work,
            // however, .NET 9 requires this type. See GitHub issue https://github.com/dotnet/efcore/issues/35126
            // Also, see resolution https://jackyasgar.net/solved-ef-8-to-9-migration-database-provider-exception/
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<ProgressVisualiserApiContext>));
            
            // Remove ALL OpenTelemetry related services before testing
            var descriptors = services.Where(d => 
                d.ImplementationType?.Namespace?.Contains("OpenTelemetry") == true ||
                d.ServiceType.FullName?.Contains("OpenTelemetry") == true
            ).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.RemoveAll<ILoggerProvider>();

            // Add DbContext using an in-memory database for testing
            services.AddDbContext<ProgressVisualiserApiContext>((container, options) =>
            {
                // in-memory may not perfectly replicate the SQL Server behavior,
                // however, this app was built with the free Azure SQL server in mind.
                // this is not permanently available for testing.
                // The in-memory db is an alternative to test basic CRUD operations.
                options.UseInMemoryDatabase("ProgressVisualiserApiInMemoryDb");
            });

            // Create a new service provider
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ProgressVisualiserApiContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<ProgressVisualiserApiWebApplicationFactory>>();

            db.Database.EnsureCreated();

            try
            {
                // seed the db
                Utilities.InitializeDbForTests(db);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                    "database with test messages. Error: {Message}", ex.Message);
            }
        });

        builder.UseEnvironment("Development");
    }
}