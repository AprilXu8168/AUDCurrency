using ExchangeRatesService.Models;

namespace ExchangeRatesService.Services;

public interface IExChangeService
{
    // you shouldn't be returning ActionResult from services, as that is a type specific to Controllers
    Task<List<CurrencyPair>> GetCurrencyPairs();
    Task<List<CurrencyPair>> FetchRateContent();

    /// <summary>
    /// Returning a CurrencyPair is not guaranteed. Can return null
    /// </summary>
    Task<CurrencyPair?> GetCurrencyPair(int id);
}