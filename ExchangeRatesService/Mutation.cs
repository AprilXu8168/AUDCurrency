using ExchangeRatesService.Models;

public class Mutation
{
    private readonly CurrenciesDBContext _context;

    public Mutation(CurrenciesDBContext context)
    {
        _context = context;
    }

    // public async Task<CurrencyPair>? AddCurrencyPair(CurrencyRates currency)
    // {
    public async Task<CurrencyPair>? AddCurrencyPair(CurrencyPair currency)
    {
    
        // Get the greatest ID currently in the database
        var maxId = _context.CurrencyPairs.Max(cp => (int?)cp.ID) ?? 0;

        // Increment the ID by 1 for the new item
        var newId = maxId;

        newId += 1;
        var newItem = new CurrencyPair(){
            ID = newId,
            Timestamp=DateTimeOffset.UtcNow,
            Name= currency.Name,
            Value = currency.Value,
        };
        Console.WriteLine("Current item: {0}", newItem.Name);

        // Add the new item to the database
        _context.CurrencyPairs.Add(newItem);
        await _context.SaveChangesAsync();

        return newItem;
    }

}

// testing mutation
// mutation {
//   addCurrencyPair(
//     currency: { name: "USD", value: 1.23, timestamp: "2024-08-27 06:41:47.262663+00" }
//   ) {
//     name
//     timestamp
//     value
//     id
//   }
// }
