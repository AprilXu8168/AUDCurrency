using Microsoft.EntityFrameworkCore;
   
    namespace ExchangeRatesService.Models;

    public class CurrenciesDBContext : DbContext
    {
        public CurrenciesDBContext(DbContextOptions<CurrenciesDBContext> options)
            : base(options)
        {
        }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
    }