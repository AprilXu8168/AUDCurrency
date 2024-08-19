using System.ComponentModel.DataAnnotations;

namespace audBackEnd.Models;

public class CurrencyItem
{
    public int ID  {get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string? Name{ get; set; }
    public string? moneyCode{ get; set; }
    public float baseValue { get; set; }
    public float value { get; set; }
}