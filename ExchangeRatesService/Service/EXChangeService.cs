
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ExchangeRatesService.Models;

namespace ExchangeRatesService.Services;
    public class EXChangeService : IEXChangeService
    {
        private readonly CurrenciesDBContext _context;

        public EXChangeService(CurrenciesDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<CurrencyPair>>> GetCurrencyPairs()
        {
            return await _context.CurrencyPairs.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<CurrencyPair>>> FetchRateContent()
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
            List<CurrencyPair> currencyPairs = new List<CurrencyPair>();

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
                        currencyPairs.Add(newItem);
                        Console.WriteLine("Insert currency of: {0}: {1}--{2}", newItem.ID, newItem.Name, newItem.Value);
                    }
                    else{
                        Console.WriteLine("id dulicated, insert failed, current maxium id: {0}", maxId);
                    }
                }
            }
            
            return currencyPairs;
        }
    
        public async Task<ActionResult<CurrencyPair>> GetCurrencyPair(int id)
        {
            var currencyPair = await _context.CurrencyPairs.FindAsync(id);

            if (currencyPair == null)
            {
                return null;
            }

            return currencyPair;
        }

        // private bool CurrencyPairExists(int id)
        // {
        //     return _context.CurrencyPairs.Any(e => e.ID == id);
        // }
    }