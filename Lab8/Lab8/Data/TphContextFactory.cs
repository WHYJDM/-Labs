using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lab8.Data
{
    public class TphContextFactory : IDesignTimeDbContextFactory<TphContext>
    {
        public TphContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TphContext>();
            builder.UseSqlite("Data Source=app.db");
            return new TphContext(builder.Options);
        }
    }
}
