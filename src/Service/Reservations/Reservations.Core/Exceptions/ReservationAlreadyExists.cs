using Funfair.Shared.Exception;

namespace Reservations.Core.Exceptions;

public class ReservationAlreadyExists(Guid id) : CoreException($"Reservation with draft id {id} already exists")
{
    public override string ErrorMessage => "reservation_already_exists";
}