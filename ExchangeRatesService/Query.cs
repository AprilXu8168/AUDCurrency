using ExchangeRatesService.Models;
// it's not necessary to add [Service] if you have registered the services at startup
public class Query
{
    [UseProjection]
    // adds the ability to add a where: {...} clause in the graphQl
    [UseFiltering]
    // query e.g. 
        // query {
        //   currencyPairs(where: { id: { lt: 50 } }) {
        //     id
        //     name
        //     timestamp
        //     value
        //   }
        // }

    [UseSorting]
    // adds the ability to add a order: {...} clause in the graphQl
        // query {
        //   currencyPairs(order: { name: ASC }) {
        //     id
        //     name
        //     timestamp
        //     value
        //   }
        // }

    public IQueryable<CurrencyPair> GetCurrencyPairs(CurrenciesDBContext context) =>
        context.CurrencyPairs;

    // use to avoid returning an array with a single entry ([{}] vs just {})
    [UseSingleOrDefault]
    public IQueryable<CurrencyPair> GetCurrencyPair(int id, CurrenciesDBContext context) =>
        context.CurrencyPairs.Where(cp => cp.ID == id);

    public IQueryable<CurrencyPair> GetCurrencyPairByName(string name, CurrenciesDBContext context) =>
        context.CurrencyPairs.Where(cp => cp.Name == name);
}