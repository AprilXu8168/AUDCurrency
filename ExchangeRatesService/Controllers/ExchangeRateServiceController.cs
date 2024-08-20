using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

using ExchangeRatesService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace ExchangeRatesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateServiceController : Controller
    {
        private readonly CurrenciesDBContext _context;

        public ExchangeRateServiceController(CurrenciesDBContext context)
        {
            _context = context;
        }

        // GET: api/ExchangeRateService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyPair>>> GetCurrencyPairs()
        {
            return await _context.CurrencyPairs.ToListAsync();
        }

  // GET: api/fetchratecontent
     [HttpGet("update_Rates")]
    public async Task<IActionResult> FetchRateContent()
    {
        // Fetch json from public api
        string url = "http://api.currencyapi.com/v3/latest"; 
        string apikey = "fca_live_fcxICI1hMR8xzFktbwu0P9mDaJlCwwgHpcHhiUsY";
        string content = "";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string tar = url+"?apikey="+apikey+"&currencies=&base_currency=AUD";
                Console.WriteLine("target url:{0}", tar);
                HttpResponseMessage response = await client.GetAsync(tar);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                // Handle errors, log them, etc.
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                content = "Error fetching website content.";
            }
        }
 

        CurrencyRates currency = JsonConvert.DeserializeObject<CurrencyRates>(content); 
        Console.Write(content); 
        var maxId = _context.CurrencyPairs.Max(ci => (int?)ci.ID) ?? 0;
        var newId = maxId;
        if (currency != null)
        {
            foreach (var pair in currency.Data.Values)
            {    
                newId += 1;
                var newItem = new CurrencyPair(){
                    ID = newId,
                    Timestamp=DateTimeOffset.UtcNow,
                    Name= pair.Code,
                    Value = pair.Value,
                };

                var existingItem = await _context.CurrencyPairs.FindAsync(newItem.ID);
                if (existingItem == null)
                {
                    _context.CurrencyPairs.Add(newItem);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Insert currency of: {0}: {1}--{2}", newItem.ID, newItem.Name, newItem.Value);
                }
                else{
                    Console.WriteLine("id dulicated, insert failed, current maxium id: {0}", maxId);
                }
            }
        }
        
        return Json(currency);
    }
 


        // GET: api/ExchangeRateService/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyPair>> GetCurrencyPair(int id)
        {
            var currencyPair = await _context.CurrencyPairs.FindAsync(id);

            if (currencyPair == null)
            {
                return NotFound();
            }

            return currencyPair;
        }

        // PUT: api/ExchangeRateService/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrencyPair(int id, CurrencyPair currencyPair)
        {
            if (id != currencyPair.ID)
            {
                return BadRequest();
            }

            _context.Entry(currencyPair).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyPairExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExchangeRateService
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CurrencyPair>> PostCurrencyPair(CurrencyPair currencyPair)
        {
            _context.CurrencyPairs.Add(currencyPair);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurrencyPair", new { id = currencyPair.ID }, currencyPair);
        }

        // DELETE: api/ExchangeRateService/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrencyPair(int id)
        {
            var currencyPair = await _context.CurrencyPairs.FindAsync(id);
            if (currencyPair == null)
            {
                return NotFound();
            }

            _context.CurrencyPairs.Remove(currencyPair);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurrencyPairExists(int id)
        {
            return _context.CurrencyPairs.Any(e => e.ID == id);
        }
    }
}
