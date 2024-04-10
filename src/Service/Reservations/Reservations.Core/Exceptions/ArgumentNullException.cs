using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class ArgumentCoreNullException(string value) : CoreException($"Argument {value} cannot be null")
{
    public override string ErrorMessage => "argument_null_exception";
}