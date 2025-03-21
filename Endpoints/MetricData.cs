using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Contexts;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Endpoints
{
    public static class MetricDataEndpoints
    {
        public static void RegisterMetricDataEndpoints(this WebApplication app)
        {
            var metricData = app.MapGroup("/metricdata");
            MapMetricDataGroup(metricData);
        }

        static void MapMetricDataGroup(RouteGroupBuilder metricData)
        {
            metricData.MapGet("/", GetAllMetricData);
            metricData.MapGet("/{id}", GetMetricData);
            metricData.MapPost("/", CreateMetricData);
            metricData.MapPut("/{id}", UpdateMetricData);
            metricData.MapDelete("/{id}", DeleteMetricData);
        }

        static async Task<IResult> GetAllMetricData(MetricDataContext db)
        {
            return TypedResults.Ok(await db.MetricData.ToArrayAsync());
        }

        static async Task<IResult> GetMetricData(int id, MetricDataContext db)
        {
            return await db.MetricData.FindAsync(id)
                is MetricData metricData
                    ? TypedResults.Ok(metricData)
                    : TypedResults.NotFound();
        }

        static async Task<IResult> CreateMetricData(MetricData metricData, MetricDataContext db)
        {
            db.MetricData.Add(metricData);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/metricdata/{metricData.Id}", metricData);
        }

        static async Task<IResult> UpdateMetricData(int id, MetricData inputMetricData, MetricDataContext db)
        {
            var metricData = await db.MetricData.FindAsync(id);

            if (metricData is null) return TypedResults.NotFound();

            metricData.Value = inputMetricData.Value;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteMetricData(int id, MetricDataContext db)
        {
            if (await db.MetricData.FindAsync(id) is MetricData metricData)
            {
                db.MetricData.Remove(metricData);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
