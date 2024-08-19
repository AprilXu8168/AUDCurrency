namespace audBackEnd.Models;
public class CurrencyRates
{
    public Meta Meta { get; set; }
    public Dictionary<string, Currency> Data { get; set; }
}

public class Meta
{
    public DateTimeOffset LastUpdatedAt { get; set; }
}

public class Currency
{
    public string Code { get; set; }
    public decimal Value { get; set; }
}