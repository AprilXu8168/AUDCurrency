using GraphQL.Types;
using ExchangeRatesService.Models;
using ExchangeRatesService.Services;

namespace ExchangeRatesService.Graphql;

public class ExchangeRatesQuery : ObjectGraphType
{
    public ExchangeRatesQuery(RatesService currencyService)
    {
        Field<ListGraphType<ExchangeRatesType>>(
            "currencyPairsList",
            resolve: context => currencyService.GetAllCurrencyPairs()
        );

        Field<ExchangeRatesType>(
            "currencyPair",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
            resolve: context => currencyService.GetCurrencyPair(context.GetArgument<int>("id"))
        );
    }
}
