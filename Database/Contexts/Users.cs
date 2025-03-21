using Microsoft.EntityFrameworkCore;
using ProgressVisualiserApi.Database.Models;

namespace ProgressVisualiserApi.Database.Contexts
{
    public class UsersContext(DbContextOptions<UsersContext> options) : DbContext(options)
    {
        public DbSet<User> Metrics { get; set; }
    }
}
