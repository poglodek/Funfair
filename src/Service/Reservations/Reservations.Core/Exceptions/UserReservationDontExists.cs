using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class UserReservationDontExists(string value) : CoreException(value)
{
    public override string ErrorMessage => "user_reservation_dont_exist";
}