using Funfair.Shared.Exception;

namespace Users.Core.Exceptions;

public class InvalidDateException : CoreException
{
    public InvalidDateException(DateTime value) : base($"Value {value} is not a valid date")
    {
    }

    public override string ErrorMessage => "invalid_date";
}