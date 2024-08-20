using GraphQL.Types;
using ExchangeRatesService.Models;

public class ExchangeRatesType : ObjectGraphType<CurrencyPair>
{
    public ExchangeRatesType()
    {
        Field(x => x.ID).Description("The ID of the currency pair.");
        Field(x => x.Timestamp).Description("The timestamp of the currency pair.");
        Field(x => x.Name).Description("The name of the currency pair.");
        Field(x => x.Value).Description("The value of the currency pair.");
    }
}
