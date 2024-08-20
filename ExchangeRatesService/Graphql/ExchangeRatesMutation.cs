using GraphQL.Types;
using ExchangeRatesService.Models;
using ExchangeRatesService.Services;

public class ExchangeRatesMutation : ObjectGraphType
{
    public ExchangeRatesMutation(RatesService currencyService)
    {
        Field<ExchangeRatesType>(
            "addCurrency",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<AddCurrencyInput>> { Name = "input" }
            ),
            resolve: context =>
            {
                var input = context.GetArgument<AddCurrencyInput>("input");
                return currencyService.AddCurrencyPair(new CurrencyPair
                {
                    Timestamp = DateTimeOffset.Parse(input.Timestamp),
                    Name = input.Name,
                    Value = input.Value
                });
            }
        );
    }
}
