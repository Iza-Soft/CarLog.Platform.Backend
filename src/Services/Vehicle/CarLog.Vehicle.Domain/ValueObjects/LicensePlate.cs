using CarLog.Vehicle.Domain.Common;
using CarLog.Vehicle.Domain.Validators;

namespace CarLog.Vehicle.Domain.ValueObjects;
/// <summary>
/// // Създаване
/// var bgPlate = LicensePlate.Create("СА1234ВТ", "BG");
/// var dePlate = LicensePlate.Create("B MW 123", "DE");
/// var ukPlate = LicensePlate.Create("AB12 CDE", "UK");

/// // Проверка
/// if (bgPlate.IsBulgarian())
///    Console.WriteLine("Това е български номер");

/// // Валидация
/// var isValid = "XYZ123".IsValidForCountry("US");

/// // Extension методи
/// var plate = "CA1234AB".ToLicensePlate("BG");

/// // Equals
/// var plate1 = LicensePlate.Create("CA1234AB", "BG");
/// var plate2 = LicensePlate.Create("CA1234AB", "BG");
/// Console.WriteLine(plate1 == plate2); // True
/// </summary>
public sealed record LicensePlate : ValueObject
{
    public string PlateNumber { get; }
    public string CountryCode { get; }

    private LicensePlate(string plateNumber, string countryCode)
    {
        PlateNumber = plateNumber;
        CountryCode = countryCode;
    }

    public static LicensePlate Create(string plateNumber, string countryCode)
    {
        var normalizedPlate = plateNumber.Trim().ToUpperInvariant();

        var normalizedCountry = countryCode.Trim().ToUpperInvariant();

        ValidateInput(normalizedPlate, normalizedCountry);

        var validator = CountryPlateValidatorFactory.Create(normalizedCountry);

        if (!validator.IsValid(normalizedPlate)) throw new ArgumentException($"Invalid license plate format for {normalizedCountry}: {normalizedPlate}");

        return new LicensePlate(normalizedPlate, normalizedCountry);
    }

    public static bool IsValid(string plateNumber, string countryCode)
    {
        try
        {
            Create(plateNumber, countryCode);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlateNumber;
        yield return CountryCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PlateNumber, CountryCode);
    }

    public override string ToString() => $"{PlateNumber} ({CountryCode})";

    public bool IsFromCountry(string countryCode)
    {
        return CountryCode.Equals(countryCode.Trim().ToUpperInvariant());
    }

    public bool MatchesPattern(string pattern)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(
            PlateNumber, pattern,
            System.Text.RegularExpressions.RegexOptions.None,
            TimeSpan.FromMilliseconds(100));
    }

    private static void ValidateInput(string plateNumber, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("Plate number cannot be empty", nameof(plateNumber));

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country code cannot be empty", nameof(countryCode));

        if (countryCode.Length != 2 || !countryCode.All(char.IsLetter))
            throw new ArgumentException("Country code must be 2 letters", nameof(countryCode));

        if (plateNumber.Length < 2 || plateNumber.Length > 12)
            throw new ArgumentException("Plate number must be between 2 and 12 characters", nameof(plateNumber));
    }
}