using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab7.Data;
using Lab7.Models;
using Lab7.Repositories;
using Lab7.Services;

namespace Lab7.Tests
{
    [TestClass]
    public class BusinessServiceTests
    {
        /// <summary>
        /// Настраивает в памяти SQLite-контекст с открытым соединением.
        /// Это нужно, чтобы транзакция реально работала и откат работал корректно.
        /// </summary>
        private AppDbContext CreateSqliteInMemoryContext(out SqliteConnection connection)
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var ctx = new AppDbContext(options);
            ctx.Database.EnsureCreated();
            return ctx;
        }

        [TestMethod]
        public async Task AddDrinkWithManufacturerAsync_CommitsBothEntities()
        {
            // Arrange
            var context = CreateSqliteInMemoryContext(out var conn);
            var mRepo = new ManufacturerRepository(context);
            var dRepo = new DrinkRepository(context);
            var service = new BusinessService(context, mRepo, dRepo);

            // Act
            await service.AddDrinkWithManufacturerAsync("TestM", "TestD");

            // Assert
            Assert.AreEqual(1, await context.Manufacturers.CountAsync());
            Assert.AreEqual(1, await context.Drinks.CountAsync());

            conn.Close();
        }

        [TestMethod]
        public async Task AddDrinkWithManufacturerAsync_RollsBackOnFailure()
        {
            // Arrange
            var context = CreateSqliteInMemoryContext(out var conn);
            var mRepo = new ManufacturerRepository(context);
            var dRepo = new FaultyDrinkRepository(context);
            var service = new BusinessService(context, mRepo, dRepo);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(
                () => service.AddDrinkWithManufacturerAsync("X", "Y")
            );

            // После отката в базе не должно быть ни производителя, ни напитка
            Assert.AreEqual(0, await context.Manufacturers.CountAsync());
            Assert.AreEqual(0, await context.Drinks.CountAsync());

            conn.Close();
        }

        /// <summary>
        /// Репозиторий, который симулирует сбой при добавлении напитка.
        /// Остальные методы делегирует EF-контексту.
        /// </summary>
        private class FaultyDrinkRepository : IRepository<Drink>
        {
            private readonly AppDbContext _ctx;
            public FaultyDrinkRepository(AppDbContext ctx) => _ctx = ctx;

            public async Task<IEnumerable<Drink>> GetAllAsync()
                => await _ctx.Drinks.ToListAsync();

            public async Task<Drink?> GetByIdAsync(int id)
                => await _ctx.Drinks.FindAsync(id);

            public Task AddAsync(Drink entity)
                => throw new Exception("Simulated failure");

            public void Update(Drink entity)
                => _ctx.Drinks.Update(entity);

            public void Delete(Drink entity)
                => _ctx.Drinks.Remove(entity);

            public Task SaveChangesAsync()
                => _ctx.SaveChangesAsync();
        }
    }
}
