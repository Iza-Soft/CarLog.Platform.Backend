namespace CarLog.Expense.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid id) : base($"{entityName} with ID '{id}' was not found.") { }
}