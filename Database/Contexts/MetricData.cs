using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Database.Contexts
{
    public class MetricDataContext(DbContextOptions<MetricDataContext> options) : DbContext(options)
    {
        public DbSet<MetricData> MetricData { get; set; }
    }
}
