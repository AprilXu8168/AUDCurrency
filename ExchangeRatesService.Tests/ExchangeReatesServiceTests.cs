using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ExchangeRatesService.Models;
using ExchangeRatesService.Services;

[TestFixture]
public class ExchangeReatesServiceTests
{
    private CurrenciesDBContext _context;
    private ExChangeService _exchangeRatesServer;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CurrenciesDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new CurrenciesDBContext(options);
        _exchangeRatesServer = new ExChangeService(_context);

        // Seed the database with test data
        _context.CurrencyPairs.AddRange(
            new CurrencyPair
            {
                ID = 1,
                Timestamp = DateTimeOffset.Parse("2024-08-29T23:59:59Z"),
                Name = "CNY",
                Value = (float)4.776m
            },
            new CurrencyPair
            {
                ID = 2,
                Timestamp = DateTimeOffset.Parse("2024-08-29T23:59:59Z"),
                Name = "JPY",
                Value = (float)430m
            }
        );

        _context.SaveChanges();
    }

    [Test]
    public async Task db_IsNot_Empty()
    {
        // Act
        var currencyPairs = await _context.CurrencyPairs.ToListAsync();
        // Assert
        foreach (var currencyPair in currencyPairs)
        {
            Console.WriteLine($"test No.1 From DB Context: \nID: {currencyPair.ID}, Timestamp: {currencyPair.Timestamp}, Name: {currencyPair.Name}, Value: {currencyPair.Value}");
        }
        Assert.That(currencyPairs, Is.Not.Empty, "DB Failed, please check");
    }

    [Test]
    public async Task IsServer_ReturnData()
    {   
        Task<List<CurrencyPair>> ret = _exchangeRatesServer.GetCurrencyPairs();
        List<CurrencyPair> currencyPairs  = await ret;
        foreach (var currencyPair in currencyPairs)
        {
            Console.WriteLine($"test No.2 From Exchange Server: \nID: {currencyPair.ID}, Timestamp: {currencyPair.Timestamp}, Name: {currencyPair.Name}, Value: {currencyPair.Value}");
        }        
        Assert.That(currencyPairs, Is.Not.Null, "Result should not be Null");
    }

    [Test]
    public async Task IsServer_Search_Accurate()
    {   
        Task<CurrencyPair> ret = _exchangeRatesServer.GetCurrencyPair(id: 2);
        CurrencyPair currencyPair  = await ret;

        Console.WriteLine($"test No.3 Search with Exchange Server: \nID: {currencyPair.ID}, Timestamp: {currencyPair.Timestamp}, Name: {currencyPair.Name}, Value: {currencyPair.Value}");
        Assert.That(currencyPair, Is.Not.Null, "Result should not be Null");
    }

    [TearDown]
    public void Teardown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
