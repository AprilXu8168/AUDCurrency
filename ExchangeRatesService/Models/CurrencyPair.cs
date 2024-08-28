using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesService.Models;

public class CurrencyPair
{
    [Key]
    public int? ID  {get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public string? Name{ get; set; }
    public float Value { get; set; }
}