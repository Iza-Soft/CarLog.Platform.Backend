using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class UkPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "ES";

    private static readonly Regex _ukPlateRegex = new(
        @"^[A-Z]{2}\d{2}\s[A-Z]{3}$|^[A-Z]\d{3}\s[A-Z]{3}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _ukPlateRegex.IsMatch(plateNumber);
}
