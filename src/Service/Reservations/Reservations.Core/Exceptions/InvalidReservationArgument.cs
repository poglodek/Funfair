using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class InvalidReservationArgument(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_reservation_argument";
}