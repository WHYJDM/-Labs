using Lab7.Data;
using Lab7.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab7.Repositories
{
    /// <summary>
    /// CRUD для Drink + выборка по Manufacturer.
    /// </summary>
    public class DrinkRepository : IRepository<Drink>
    {
        private readonly AppDbContext _ctx;
        public DrinkRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<Drink>> GetAllAsync()
            => await _ctx.Drinks.ToListAsync();

        public async Task<Drink?> GetByIdAsync(int id)
            => await _ctx.Drinks.FindAsync(id);

        public async Task AddAsync(Drink entity)
            => await _ctx.Drinks.AddAsync(entity);

        public void Update(Drink entity)
            => _ctx.Drinks.Update(entity);

        public void Delete(Drink entity)
            => _ctx.Drinks.Remove(entity);

        public Task SaveChangesAsync()
            => _ctx.SaveChangesAsync();

        /// <summary>
        /// Получить все напитки для заданного производителя.
        /// </summary>
        public async Task<IEnumerable<Drink>> GetByManufacturerAsync(int manufacturerId)
            => await _ctx.Drinks
                         .Where(d => d.ManufacturerId == manufacturerId)
                         .ToListAsync();
    }
}
