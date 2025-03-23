using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Database
{
    public class ProgressVisualiserApiContext(DbContextOptions<ProgressVisualiserApiContext> options) : DbContext(options)
    {
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricData> MetricData { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
