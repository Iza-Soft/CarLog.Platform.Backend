using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.NorthAmerica;

public sealed class UsPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "US";

    private static readonly Regex _usPlateRegex = new(
        @"^[A-Z0-9]{1,7}$|^[A-Z0-9]{1,6}[-\s][A-Z0-9]{1,6}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _usPlateRegex.IsMatch(plateNumber);
}