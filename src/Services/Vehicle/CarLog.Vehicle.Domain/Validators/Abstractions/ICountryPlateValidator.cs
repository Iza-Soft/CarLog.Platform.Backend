namespace CarLog.Vehicle.Domain.Validators.Abstractions;

public interface ICountryPlateValidator
{
    string CountryCode { get; }
    bool IsValid(string plateNumber);
}