using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lab8.Data
{
    public class TptContextFactory : IDesignTimeDbContextFactory<TptContext>
    {
        public TptContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TptContext>();
            builder.UseSqlite("Data Source=app.db");
            return new TptContext(builder.Options);
        }
    }
}
