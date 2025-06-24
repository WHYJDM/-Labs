using Lab7.Data;
using Lab7.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab7.Repositories
{
    /// <summary>
    /// CRUD для Manufacturer.
    /// </summary>
    public class ManufacturerRepository : IRepository<Manufacturer>
    {
        private readonly AppDbContext _ctx;
        public ManufacturerRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Manufacturer>> GetAllAsync()
            => await _ctx.Manufacturers.ToListAsync();

        public async Task<Manufacturer?> GetByIdAsync(int id)
            => await _ctx.Manufacturers.FindAsync(id);

        public async Task AddAsync(Manufacturer entity)
            => await _ctx.Manufacturers.AddAsync(entity);

        public void Update(Manufacturer entity)
            => _ctx.Manufacturers.Update(entity);

        public void Delete(Manufacturer entity)
            => _ctx.Manufacturers.Remove(entity);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();
    }
}
