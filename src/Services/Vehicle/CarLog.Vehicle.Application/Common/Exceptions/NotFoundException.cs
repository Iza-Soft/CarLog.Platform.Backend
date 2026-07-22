namespace CarLog.Vehicle.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid id) : base($"{entityName} with ID '{id}' can not be found.") { }
}