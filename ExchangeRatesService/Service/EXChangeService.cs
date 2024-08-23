using ExchangeRatesService.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

// use namespaces like this to avoid unnecessary nesting
namespace ExchangeRatesService.Services;

public class ExChangeService(CurrenciesDBContext db) : IExChangeService
{
    public Task<List<CurrencyPair>> GetCurrencyPairs()
    {
        return db.CurrencyPairs.ToListAsync();
    }

    public async Task<List<CurrencyPair>> FetchRateContent()
    {
        // by using const (constants) we avoid allocating strings
        const string baseUrl = "http://api.currencyapi.com/v3/latest"; 
        const string apikey = "fca_live_fcxICI1hMR8xzFktbwu0P9mDaJlCwwgHpcHhiUsY";
        const string requestUrl = $"{baseUrl}?apikey={apikey}&currencies=&base_currency=AUD";
        
        Console.WriteLine($"target url:{requestUrl}");
        
        string content;
        
        try
        {
            // Fetch json from public api
            var response = await Program.httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            content = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            // Handle errors, log them, etc.
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message} ");
            content = "Error fetching website content.";
        }

        // use System.Text.Json rather than Newtonsoft, unless there is something (rare) that the framework JSON API cannot do
        var currency = JsonSerializer.Deserialize<CurrencyRates>(content);

        var currencyPairs = new List<CurrencyPair>();

        if (currency == null)
        {
            return currencyPairs;
        }
        
        // ID is defined as not nullable
        var maxId = db.CurrencyPairs.Max(ci => ci.ID);
        var newId = maxId;
        
        foreach (var pair in currency.Data.Values)
        {    
            newId += 1;
            var newItem = new CurrencyPair
            {
                ID = newId,
                Timestamp=DateTimeOffset.UtcNow,
                Name= pair.Code,
                Value = pair.Value,
            };
            
            Console.WriteLine($"Current item: {newItem.Name}");

            var existingItem = await db.CurrencyPairs.FindAsync(newItem.ID);
            try
            {
                if (existingItem == null)
                {
                    // Add the new item to the context
                    db.CurrencyPairs.Add(newItem);

                    // Attempt to save changes to the database
                    await db.SaveChangesAsync();

                    // Add the new item to the list
                    currencyPairs.Add(newItem);

                    // Log success message
                    Console.WriteLine($"Inserted currency: {newItem.ID}: {newItem.Name}--{newItem.Value}");
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

        return currencyPairs;
    }

    // fixed nullability
    public async Task<CurrencyPair?> GetCurrencyPair(int id)
    {
        return await db.CurrencyPairs.FindAsync(id);
    }
}