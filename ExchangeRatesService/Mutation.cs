using ExchangeRatesService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class Mutation
{
    // private readonly CurrenciesDBContext _context;
    private readonly IServiceScopeFactory _scopeFactory;

    public Mutation(IServiceScopeFactory scopeFactory)
    {
         _scopeFactory = scopeFactory;
    }
    public async Task<List<CurrencyPair>>? FetchnUpdate()
    {
        // Fetch json from public api
        string url = "http://api.currencyapi.com/v3/latest"; 
        string apikey = "cur_live_T7700pGjZzpJbp0zN5s6Whnwj4fDo2ZttCGViU0r";
        
        string content = "";
        using (HttpClient client = new HttpClient())
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<CurrenciesDBContext>();
                    string tar = url+"?apikey="+apikey+"&currencies=&base_currency=AUD";
                    Console.WriteLine("target url:{0}", tar);
                    HttpResponseMessage response = await client.GetAsync(tar);
                    response.EnsureSuccessStatusCode();
                    content = await response.Content.ReadAsStringAsync();
                }
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

        List<CurrencyPair> currencyPairs = new List<CurrencyPair>();

        if (currency != null)
        {
            foreach (var pair in currency.Data.Values)
            {
                var pairItem = new CurrencyPair(){
                    Name= pair.Code,
                    Value= pair.Value,

                };
                await AddCurrencyPair(pairItem);
                currencyPairs.Add(pairItem);
            };
            Console.WriteLine("Fetched and added {0} items into DB",currencyPairs.Count());
        }
        return currencyPairs;
    }

    public async Task<CurrencyPair?> AddCurrencyPair(CurrencyPair currency)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<CurrenciesDBContext>();
                
                // Get the greatest ID currently in the database
                var maxId = _context.CurrencyPairs.Max(cp => (int?)cp.ID) ?? 0;

                // Increment the ID by 1 for the new item
                var newId = maxId + 1;

                var newItem = new CurrencyPair()
                {
                    ID = newId,
                    Timestamp = DateTimeOffset.UtcNow,
                    Name = currency.Name,
                    Value = currency.Value,
                };

                // Add the new item to the database
                _context.CurrencyPairs.Add(newItem);
                await _context.SaveChangesAsync();
                Console.WriteLine("Inserted currency by GraphQL api: {0}: {1}--{2}", newItem.ID, newItem.Name, newItem.Value);

                return newItem;
            }
        }
        catch (DbUpdateException ex)
        {
            // Handle database update errors specifically
            Console.WriteLine($"Database update error: {ex.Message}");
            throw new Exception("An error occurred while adding the currency pair to the database. Please try again.");
        }
        catch (ArgumentNullException ex)
        {
            // Handle cases where the provided currency object is null
            Console.WriteLine($"Null argument error: {ex.Message}");
            throw new Exception("The currency data provided is invalid. Please check the input and try again.");
        }
        catch (Exception ex)
        {
            // Handle other errors
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new Exception($"Add operation failed: {ex.Message}");
        }
    }

    public async Task<CurrencyPair?> UpdateCurrencyPair(int id, CurrencyPair updatedCurrencyPair)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService< CurrenciesDBContext>();
                
                var existingItem = await _context.CurrencyPairs.FindAsync(id);
                
                if (existingItem == null)
                {
                    // If the item is not found, return null or throw an exception
                    Console.WriteLine($"CurrencyPair with ID {id} not found.");
                    return null;
                }
                // Perform the mutation
                var newItem = new CurrencyPair
                {
                    ID = id,
                    Timestamp = existingItem.Timestamp,
                    Name = updatedCurrencyPair.Name,
                    Value = updatedCurrencyPair.Value
                };
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.CurrencyPairs.Update(newItem);
                await _context.SaveChangesAsync();

                return newItem;
            }
        }
        catch (DbUpdateException ex)
        {
            // Handle database update errors specifically
            Console.WriteLine($"Database update error: {ex.Message}");
            throw new Exception("An error occurred while updating the database. Please try again.");
        }
        catch (Exception ex)
        {
            // Handle other errors
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new Exception($"Update failed: {ex.Message}");
        }
    }

    public async Task<bool> DeleteCurrencyPairAsync(int id)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<CurrenciesDBContext>();

                // Find the entity with the given ID
                var existingItem = await _context.CurrencyPairs.FindAsync(id);

                if (existingItem == null)
                {
                    // If the item is not found, log a message and return false
                    Console.WriteLine($"CurrencyPair with ID {id} not found.");
                    return false;
                }

                // Remove the entity
                _context.CurrencyPairs.Remove(existingItem);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (DbUpdateException ex)
        {
            // Handle database update errors specifically
            Console.WriteLine($"Database update error: {ex.Message}");
            throw new Exception("An error occurred while updating the database. Please try again.");
        }
        catch (Exception ex)
        {
            // Handle other errors
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw new Exception($"Delete failed: {ex.Message}");
        }
    }
}

// testing mutation
//  mutation {
//    addCurrencyPair(
//      currency: { name: "TEST", value: 0.123456}
//    ) {
//      name
//      timestamp
//      value
//      id
//    }
//  }

// mutation {
//    updateCurrencyPair(id: 3, updatedCurrencyPair: { name: "TEST", value: 1.23456789 }) {
//      id
//      name
//      timestamp
//      value
//    }
//  }
