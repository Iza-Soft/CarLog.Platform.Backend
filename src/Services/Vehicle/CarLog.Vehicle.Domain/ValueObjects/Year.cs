using CarLog.Vehicle.Domain.Common;
using CarLog.Vehicle.Domain.Exceptions;

namespace CarLog.Vehicle.Domain.ValueObjects;

/// <summary>
/// Представлява година на производство на превозно средство
/// 
/// Употреба:
/// var year = Year.Create(2023);
/// int yearValue = year; // implicit conversion
/// 
/// Валидация:
/// - Минимална година: 1900 (исторически автомобили)
/// - Максимална година: текуща + 1 (бъдещи модели)
/// </summary>
public sealed record Year : ValueObject
{
    public int Value { get; }

    private Year(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Factory method for creating a Year with validation
    /// </summary>
    public static Year Create(int year)
    {
        var currentYear = DateTime.UtcNow.Year;

        if (year < 1900 || year > currentYear + 1) throw new DomainException($"Year must be between 1900 and {currentYear + 1}. Provided: {year}");

        return new Year(year);
    }

    /// <summary>
    /// Try-Pattern for Validation without Exceptions
    /// </summary>
    public static bool TryCreate(int year, out Year? result)
    {
        var currentYear = DateTime.UtcNow.Year;

        if (year < 1900 || year > currentYear + 1)
        {
            result = null;

            return false;
        }

        result = new Year(year);

        return true;
    }

    /// <summary>
    /// Value equality components (used by the underlying ValueObject)
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator int(Year year) => year.Value;

    public static explicit operator Year(int year) => Create(year);

    public Year AddYears(int years) => new Year(Value + years);

    public bool IsFuture() => Value > DateTime.UtcNow.Year;

    public bool IsPast() => Value < DateTime.UtcNow.Year;

    public bool IsCurrent() => Value == DateTime.UtcNow.Year;
}