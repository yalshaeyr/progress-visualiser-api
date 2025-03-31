using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Controllers
{
    public static class MetricDataController
    {
        public static void RegisterMetricDataController(this WebApplication app)
        {
            var metricData = app.MapGroup("/metricdata");
            MapMetricDataGroup(metricData);
        }

        static void MapMetricDataGroup(RouteGroupBuilder metricData)
        {
            metricData.MapGet("/", GetAllMetricData);
            metricData.MapGet("/{id}", GetMetricData);
            metricData.MapGet("/metric/{metricId}", GetMetricDataByMetricId);
            metricData.MapPost("/", CreateMetricData);
            metricData.MapPut("/{id}", UpdateMetricData);
            metricData.MapDelete("/{id}", DeleteMetricData);
            metricData.MapDelete("/metric/{metricId}", DeleteMetricDataByMetricId);
        }

        static async Task<IResult> GetAllMetricData(ProgressVisualiserApiContext db)
        {
            return TypedResults.Ok(await db.MetricData.ToArrayAsync());
        }

        static async Task<IResult> GetMetricData(int id, ProgressVisualiserApiContext db)
        {
            return await db.MetricData.FindAsync(id)
                is MetricData metricData
                    ? TypedResults.Ok(metricData)
                    : TypedResults.NotFound();
        }

        static async Task<IResult> GetMetricDataByMetricId(int metricId, ProgressVisualiserApiContext db)
        {
            var metric = await db.Metrics.FindAsync(metricId);
            var metricData = await db.MetricData.Where(md => md.MetricId == metricId).ToListAsync();
            
            if (metric == null)
            {
                return TypedResults.NotFound();
            }

            return metricData.Count != 0 ? TypedResults.Ok(metricData) : TypedResults.Ok(new List<MetricData>());
        }        

        static async Task<IResult> CreateMetricData(MetricData metricData, ProgressVisualiserApiContext db)
        {
            db.MetricData.Add(metricData);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/metricdata/{metricData.Id}", metricData);
        }

        static async Task<IResult> UpdateMetricData(int id, MetricData inputMetricData, ProgressVisualiserApiContext db)
        {
            var metricData = await db.MetricData.FindAsync(id);

            if (metricData is null) return TypedResults.NotFound();

            metricData.Value = inputMetricData.Value;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteMetricData(int id, ProgressVisualiserApiContext db)
        {
            if (await db.MetricData.FindAsync(id) is MetricData metricData)
            {
                db.MetricData.Remove(metricData);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        static async Task<IResult> DeleteMetricDataByMetricId(int metricId, ProgressVisualiserApiContext db)
        {
            var metricDataList = await db.MetricData.Where(md => md.MetricId == metricId).ToListAsync();

            if (metricDataList.Count == 0)
            {
                return TypedResults.NoContent();
            }

            db.MetricData.RemoveRange(metricDataList);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}
