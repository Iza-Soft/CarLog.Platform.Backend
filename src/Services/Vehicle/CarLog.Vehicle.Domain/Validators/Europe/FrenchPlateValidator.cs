using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class FrenchPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "FR";

    private static readonly Regex _frPlateRegex = new(
        @"^\d{3}[A-Z]{3}\d{2}$|^[A-Z]{2}-\d{3}-[A-Z]{2}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _frPlateRegex.IsMatch(plateNumber);
}