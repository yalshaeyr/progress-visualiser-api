using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Contexts;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Endpoints 
{
    public static class MetricEndpoints
    {
        public static void RegisterMetricEndpoints(this WebApplication app)
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

        static async Task<IResult> GetAllMetrics(MetricsContext db)
        {
            return TypedResults.Ok(await db.Metrics.ToArrayAsync());
        }

        static async Task<IResult> GetMetric(int id, MetricsContext db)
        {
            return await db.Metrics.FindAsync(id)
                is Metric metric
                    ? TypedResults.Ok(metric)
                    : TypedResults.NotFound();
        }

        static async Task<IResult> CreateMetric(Metric metric, MetricsContext db)
        {
            db.Metrics.Add(metric);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/metrics/{metric.Id}", metric);
        }

        static async Task<IResult> UpdateMetric(int id, Metric inputMetric, MetricsContext db)
        {
            var metric = await db.Metrics.FindAsync(id);

            if (metric is null) return TypedResults.NotFound();

            metric.Name = inputMetric.Name;
            metric.Description = inputMetric.Description;
            metric.Unit = inputMetric.Unit;


            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteMetric(int id, MetricsContext db)
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