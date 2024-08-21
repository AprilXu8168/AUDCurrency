using System.Linq;
using HotChocolate;
using ExchangeRatesService.Models;
public class Query
{
    public string Hello(string name = "World")
        =>$"Hello, {name}!";

    public IQueryable<CurrencyPair> GetCurrencyPairs([Service] CurrenciesDBContext context) =>
        context.CurrencyPairs;

    public IQueryable<CurrencyPair> GetCurrencyPair(int id,[Service] CurrenciesDBContext context) =>
        context.CurrencyPairs.Where(cp => cp.ID == id);

    public IQueryable<CurrencyPair> GetCurrencyPairByName(string name,[Service] CurrenciesDBContext context) =>
        context.CurrencyPairs.Where(cp => cp.Name == name);
}