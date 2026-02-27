namespace CarLog.Vehicle.Domain.ValueObjects;

public static class LicensePlateExtensions
{
    public static bool IsBulgarian(this LicensePlate plate)
        => plate.CountryCode == "BG";

    public static bool IsEuropean(this LicensePlate plate)
    {
        var euCountries = new[] { "BG", "DE", "RO", "GR", "IT", "FR", "ES", "UK" };
        return euCountries.Contains(plate.CountryCode);
    }

    public static bool IsValidForCountry(this string plateNumber, string countryCode)
        => LicensePlate.IsValid(plateNumber, countryCode);

    public static LicensePlate ToLicensePlate(this string plateNumber, string countryCode)
        => LicensePlate.Create(plateNumber, countryCode);
}