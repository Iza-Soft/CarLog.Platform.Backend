using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class GermanPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "DE";

    private static readonly Regex _dePlateRegex = new(
        @"^[A-Z]{1,3}[A-Z]{1,2}\d{1,4}[A-Z]?$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _dePlateRegex.IsMatch(plateNumber);
}