using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Database.Models;
using ProgressVisualiserApi.Services;

namespace ProgressVisualiserApi.Controllers 
{
    public static class MetricController
    {
        public static void RegisterMetricController(this WebApplication app)
        {
            var metrics = app.MapGroup("/metrics");
            MapMetricGroup(metrics);
        }

        static void MapMetricGroup(RouteGroupBuilder metrics)
        {
            metrics.MapGet("/", GetAllMetrics);
            metrics.MapGet("/{id}", GetMetric);
            metrics.MapPost("/", CreateMetric);
            metrics.MapPut("/{id}", UpdateMetric);
            metrics.MapDelete("/{id}", DeleteMetric);
        }

        static async Task<IResult> GetAllMetrics(ProgressVisualiserApiContext db)
        {
            return TypedResults.Ok(await db.Metrics.ToArrayAsync());
        }

        static async Task<IResult> GetMetric(int id, ProgressVisualiserApiContext db)
        {
            return await db.Metrics.FindAsync(id)
                is Metric metric
                    ? TypedResults.Ok(metric)
                    : TypedResults.NotFound();
        }

        static async Task<IResult> CreateMetric(Metric metric, ProgressVisualiserApiContext db, IContentSafetyService contentSafetyService)
        {
            // Analyse metric name and description - one string to avoid multiple calls to the service
            if (!await contentSafetyService.IsContentSafeAsync(metric.Name + " " + metric.Description + " " + metric.Unit))
            {
                return TypedResults.BadRequest("Metric name or description contains unsafe content.");
            }

            // Ensure the ID is not set by the client, as the database will handle it
            metric.Id = 0;

            // TODO: Remove - Once Users are implemented, the client should specify the user.
            // Using this for now as there is only one user which the client cannot yet specify.
            metric.UserId = 1;

            db.Metrics.Add(metric);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/metrics/{metric.Id}", metric);
        }

        static async Task<IResult> UpdateMetric(int id, Metric inputMetric, ProgressVisualiserApiContext db, IContentSafetyService contentSafetyService)
        {
            var metric = await db.Metrics.FindAsync(id);

            if (metric is null) return TypedResults.NotFound();

            if (!await contentSafetyService.IsContentSafeAsync(inputMetric.Name + " " + inputMetric.Description + " " + inputMetric.Unit))
            {
                return TypedResults.BadRequest("Metric name or description contains unsafe content.");
            }

            metric.Name = inputMetric.Name;
            metric.Description = inputMetric.Description;
            metric.Unit = inputMetric.Unit;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteMetric(int id, ProgressVisualiserApiContext db)
        {
            if (await db.Metrics.FindAsync(id) is Metric metric)
            {
                db.Metrics.Remove(metric);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}