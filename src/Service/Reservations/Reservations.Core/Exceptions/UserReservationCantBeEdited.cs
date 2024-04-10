using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class UserReservationCantBeEdited(string message) : CoreException(message)
{
    public override string ErrorMessage => "user_reservation_cant_be_edited";
}