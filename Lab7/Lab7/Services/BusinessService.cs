using System;
using System.Threading.Tasks;
using Lab7.Data;
using Lab7.Models;
using Lab7.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lab7.Services
{
    /// <summary>
    /// Сервисная логика с поддержкой транзакции.
    /// </summary>
    public class BusinessService
    {
        private readonly AppDbContext _ctx;
        private readonly IRepository<Manufacturer> _mRepo;
        private readonly IRepository<Drink> _dRepo;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="context">EF-контекст</param>
        /// <param name="manufacturerRepository">Репозиторий производителей</param>
        /// <param name="drinkRepository">Репозиторий напитков</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BusinessService(
            AppDbContext context,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<Drink> drinkRepository)
        {
            _ctx = context ?? throw new ArgumentNullException(nameof(context));
            _mRepo = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));
            _dRepo = drinkRepository ?? throw new ArgumentNullException(nameof(drinkRepository));
        }

        /// <summary>
        /// Добавляет производителя и напиток в одной транзакции.
        /// При ошибке – откат.
        /// </summary>
        /// <param name="manufacturerName">Имя производителя</param>
        /// <param name="drinkName">Имя напитка</param>
        public async Task AddDrinkWithManufacturerAsync(string manufacturerName, string drinkName)
        {
            await using var tx = await _ctx.Database.BeginTransactionAsync();
            try
            {
                var manufacturer = new Manufacturer { Name = manufacturerName };
                await _mRepo.AddAsync(manufacturer);
                await _mRepo.SaveChangesAsync();

                var drink = new Drink
                {
                    Name = drinkName,
                    ManufacturerId = manufacturer.Id
                };
                await _dRepo.AddAsync(drink);
                await _dRepo.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
