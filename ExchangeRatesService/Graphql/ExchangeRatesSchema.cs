using GraphQL.Types;

namespace ExchangeRatesService.Graphql
{
    public class ExchangeRatesSchema : HotChocolate.Schema
    {
        public ExchangeRatesSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<ExchangeRatesQuery>();
            Mutation = provider.GetRequiredService<ExchangeRatesMutation>();
        }
    }
}