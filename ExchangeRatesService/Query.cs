using ExchangeRatesService.Models;

// it's not necessary to add [Service] if you have registered the services at startup
// added (and installed nuget package) attributes from hotchocolate.data.entityFramework
public class Query
{
    public string Hello(string name = "World") => $"Hello, {name}!";

    // automatically optimizes database query
    [UseProjection]
    // adds the ability to add a where: {...} clause in the graphQl
    [UseFiltering]
    // adds the ability to add a order: {...} clause in the graphQl
    [UseSorting]
    public IQueryable<CurrencyPair> GetCurrencyPairs(CurrenciesDBContext context) =>
        context.CurrencyPairs;

    // use to avoid returning an array with a single entry ([{}] vs just {})
    // [UseSingleOrDefault]
    public IQueryable<CurrencyPair> GetCurrencyPair(int id, CurrenciesDBContext context) =>
        context.CurrencyPairs.Where(cp => cp.ID == id);

    public IQueryable<CurrencyPair> GetCurrencyPairByName(
        string name,
        CurrenciesDBContext context
    ) => context.CurrencyPairs.Where(cp => cp.Name == name);
}
