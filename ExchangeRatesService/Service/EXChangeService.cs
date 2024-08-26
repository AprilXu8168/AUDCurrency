
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
            string apikey = "cur_live_T7700pGjZzpJbp0zN5s6Whnwj4fDo2ZttCGViU0r";
            
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
            // Console.Write(content); 
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
                    Console.WriteLine("Current item: {0}", newItem.Name);

                    var existingItem = await _context.CurrencyPairs.FindAsync(newItem.ID);
                    try
                    {
                        if (existingItem == null)
                        {
                            // Add the new item to the context
                            _context.CurrencyPairs.Add(newItem);

                            // Attempt to save changes to the database
                            await _context.SaveChangesAsync();

                            // Add the new item to the list
                            currencyPairs.Add(newItem);

                            // Log success message
                            Console.WriteLine("Inserted currency: {0}: {1}--{2}", newItem.ID, newItem.Name, newItem.Value);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Handle specific exceptions like InvalidOperationException
                        Console.WriteLine($"Invalid operation error: {ex.Message}");
                        // Optionally, rethrow or handle further
                    }
                    catch (DbUpdateException ex)
                    {
                        // Handle exceptions related to database updates
                        Console.WriteLine($"Database update error: {ex.Message}");
                        // Optionally, rethrow or handle further
                    }
                    catch (Exception ex)
                    {
                        // Handle all other types of exceptions
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        // Optionally, rethrow or handle further
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