using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class ItalianPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "IT";

    private static readonly Regex _itPlateRegex = new(
        @"^[A-Z]{2}\d{3}[A-Z]{2}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _itPlateRegex.IsMatch(plateNumber);
}

