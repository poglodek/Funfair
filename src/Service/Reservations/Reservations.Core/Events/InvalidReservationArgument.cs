using Funfair.Shared.Exception;

namespace Reservations.Core.Events;

public class InvalidReservationArgument(string value) : CoreException(value)
{
    public override string ErrorMessage => "invalid_reservation_argument";
}