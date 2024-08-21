using Microsoft.AspNetCore.Mvc;
using ExchangeRatesService.Models;
namespace ExchangeRatesService.Services
{
    public interface IEXChangeService
    {
        Task<ActionResult<IEnumerable<CurrencyPair>>> GetCurrencyPairs();
        Task<ActionResult<IEnumerable<CurrencyPair>>> FetchRateContent();
        Task<ActionResult<CurrencyPair>> GetCurrencyPair(int id);
    }
}