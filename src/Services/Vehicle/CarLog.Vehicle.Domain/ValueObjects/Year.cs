using CarLog.Vehicle.Domain.Common;

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
    /// <param name="year">The year to validate</param>
    /// <returns>A valid Year object</returns>
    /// <exception cref="ArgumentException">Thrown on invalid year</exception>
    public static Year Create(int year)
    {
        var currentYear = DateTime.UtcNow.Year;

        if (year < 1900 || year > currentYear + 1)
            throw new ArgumentException(
                $"Year must be between 1900 and {currentYear + 1}. Provided: {year}",
                nameof(year));

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

    /// <summary>
    /// Optimized GetHashCode for better performance in collections
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    /// <summary>
    /// Readable representation for logging and debugging
    /// </summary>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Implicit conversion to int for convenience - int yearValue = year1 (Implicit conversion to int)
    /// </summary>
    public static implicit operator int(Year year) => year.Value;

    /// <summary>
    /// Explicit conversion from int (recommended to use Create() instead) - Year year4 = (Year)2023 (Explicit conversion from int)
    /// </summary>
    public static explicit operator Year(int year) => Create(year);

    /// <summary>
    /// Add years (creates a new object - immutability)
    /// </summary>
    public Year AddYears(int years) => new Year(Value + years);

    /// <summary>
    /// Check if the year is in the future
    /// </summary>
    public bool IsFuture() => Value > DateTime.UtcNow.Year;

    /// <summary>
    /// Checking if the year has passed
    /// </summary>
    public bool IsPast() => Value < DateTime.UtcNow.Year;

    /// <summary>
    /// Check if the year is current
    /// </summary>
    public bool IsCurrent() => Value == DateTime.UtcNow.Year;
}
