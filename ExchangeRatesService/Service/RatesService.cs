using ExchangeRatesService.Models;

namespace ExchangeRatesService.Services
{
    public class RatesService
    {
        private readonly CurrenciesDBContext _context;

        public RatesService(CurrenciesDBContext context)
        {
            _context = context;
        }

        public IEnumerable<CurrencyPair> GetAllCurrencyPairs()
        {
            return _context.CurrencyPairs.ToList();
        }

        public CurrencyPair? GetCurrencyPair(int id)
        {
            return _context.CurrencyPairs.Find(id);
        }

        public CurrencyPair AddCurrencyPair(CurrencyPair currencyPair)
        {
            _context.CurrencyPairs.Add(currencyPair);
            _context.SaveChanges();
            return currencyPair;
        }

        public CurrencyPair UpdateCurrencyPair(CurrencyPair currencyPair)
        {
            _context.CurrencyPairs.Update(currencyPair);
            _context.SaveChanges();
            return currencyPair;
        }

        public void DeleteCurrencyPair(int id)
        {
            var currencyPair = _context.CurrencyPairs.Find(id);
            if (currencyPair != null)
            {
                _context.CurrencyPairs.Remove(currencyPair);
                _context.SaveChanges();
            }
        }
    }
}