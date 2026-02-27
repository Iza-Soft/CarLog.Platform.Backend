using CarLog.Vehicle.Domain.Validators.Abstractions;
using CarLog.Vehicle.Domain.Validators.Europe;
using CarLog.Vehicle.Domain.Validators.Balkans;
using CarLog.Vehicle.Domain.Validators.NorthAmerica;

namespace CarLog.Vehicle.Domain.Validators;

public static class CountryPlateValidatorFactory
{
    private static readonly Dictionary<string, ICountryPlateValidator> _validators = new()
    {
        // Europe
        ["BG"] = new BulgarianPlateValidator(),
        ["DE"] = new GermanPlateValidator(),
        ["RO"] = new RomanianPlateValidator(),
        ["GR"] = new GreekPlateValidator(),
        ["IT"] = new ItalianPlateValidator(),
        ["FR"] = new FrenchPlateValidator(),
        ["ES"] = new SpanishPlateValidator(),
        ["UK"] = new UkPlateValidator(),

        // Balkans
        ["RS"] = new SerbianPlateValidator(),
        ["MK"] = new MacedonianPlateValidator(),
        ["TR"] = new TurkishPlateValidator(),

        // North America
        ["US"] = new UsPlateValidator(),
    };

    public static ICountryPlateValidator Create(string countryCode)
    {
        return _validators.TryGetValue(countryCode, out var validator)
            ? validator
            : new GenericPlateValidator();
    }

    // for DI containers (if we want to register them)
    public static IEnumerable<ICountryPlateValidator> GetAllValidators()
        => _validators.Values;
}