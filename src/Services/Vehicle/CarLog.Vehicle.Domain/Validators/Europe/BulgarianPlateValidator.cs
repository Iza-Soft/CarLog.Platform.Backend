using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class BulgarianPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "BG";

    private static readonly Regex _bgPlateRegex = new(
        @"^(?:[А-Я]{1,2}\d{4}[А-Я]{1,2}|[А-Я]{2}\d{4}[А-Я]{2}|[А-Я]\d{4}[А-Я]{2})$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _bgPlateRegex.IsMatch(plateNumber);
}