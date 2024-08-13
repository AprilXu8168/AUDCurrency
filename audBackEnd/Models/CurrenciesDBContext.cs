using Microsoft.EntityFrameworkCore;
   
    namespace audBackEnd.Models;

    public class CurrenciesDBContext : DbContext
    {
        public CurrenciesDBContext(DbContextOptions<CurrenciesDBContext> options)
            : base(options)
        {
        }
        public DbSet<CurrencyItem> CurrencyItems { get; set; }
    }