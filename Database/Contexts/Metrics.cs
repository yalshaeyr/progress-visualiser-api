using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Database.Contexts
{
    public class MetricsContext(DbContextOptions<MetricsContext> options) : DbContext(options)
    {
        public DbSet<Metric> Metrics { get; set; }
    }
}
