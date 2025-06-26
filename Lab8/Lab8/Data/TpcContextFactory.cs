using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lab8.Data
{
    /// <summary>
    /// Позволяет EF CLI создавать TpcContext для миграций.
    /// </summary>
    public class TpcContextFactory : IDesignTimeDbContextFactory<TpcContext>
    {
        public TpcContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TpcContext>();
            builder.UseSqlite("Data Source=app.db");
            return new TpcContext(builder.Options);
        }
    }
}
