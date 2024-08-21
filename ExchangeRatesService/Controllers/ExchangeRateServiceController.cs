using Microsoft.AspNetCore.Mvc;
using ExchangeRatesService.Models;
using ExchangeRatesService.Services;


namespace ExchangeRatesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesServiceController : Controller
    {
        // private readonly CurrenciesDBContext _context;
        private readonly IEXChangeService _exchangeService;

        public ExchangeRatesServiceController(IEXChangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        // GET: api/ExchangeRatesService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyPair>>> GetCurrencyPairs()
        {
            var currencies = await _exchangeService.GetCurrencyPairs();
            return Ok(currencies);
        }

        // GET: api/fetchratecontent
        [HttpGet("update_Rates")]
        public async Task<IActionResult> FetchRateContent()
        {
            var list = await _exchangeService.FetchRateContent();
            return Ok(list);
        }
    
        // GET: api/ExchangeRatesService/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyPair>> GetCurrencyPair(int id)
        {
            var currencyPair = await _exchangeService.GetCurrencyPair(id);

            if (currencyPair == null)
            {
                return NotFound();
            }

            return currencyPair;
        }
    }
}
