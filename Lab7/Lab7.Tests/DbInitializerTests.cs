using Lab7.Data;
using Lab7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab7.Tests;

[TestClass]
public class DbInitializerTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [TestMethod]
    public async Task SeedAsyncShouldAddManufacturersAndDrinks()
    {
        using var context = GetInMemoryContext();
        await DbInitializer.SeedAsync(context);

        Assert.IsTrue(context.Manufacturers.Count() >= 30);
        Assert.IsTrue(context.Drinks.Count() >= 30);
    }
}
