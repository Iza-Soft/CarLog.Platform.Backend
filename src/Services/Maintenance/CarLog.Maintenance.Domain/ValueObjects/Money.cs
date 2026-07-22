using CarLog.Maintenance.Domain.Common;
using CarLog.Maintenance.Domain.Exceptions;

namespace CarLog.Maintenance.Domain.ValueObjects;

public sealed record Money : ValueObject
{
    public decimal Amount { get; }

    public string Currency { get; }

    private Money(decimal amount, string currency) 
    { 
        Amount = amount;

        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "BGN") 
    {
        if (amount < 0) throw new DomainException("Amount can not be negative");

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3) throw new DomainException("The currency must be a 3-letter code (e.g. BGN, EUR).");

        return new Money(amount, currency.ToUpperInvariant());
    }

    public override string ToString() => $"{Amount:F2} {Currency}";

    protected override IEnumerable<object> GetEqualityComponents() 
    {
        yield return Amount;

        yield return Currency;
    }

}
