using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class UserReservationAlreadyExists(string value) : CoreException(value)
{
    public override string ErrorMessage => "user_reservation_already_exists";
}