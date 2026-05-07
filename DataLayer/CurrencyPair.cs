namespace DataLayer;

public class CurrencyPair
{
    public int Id { get; set; }
    public required string BaseCode { get; set; }
    public required string QuoteCode { get; set; }
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
    public decimal CurrentValue { get; set; }
}