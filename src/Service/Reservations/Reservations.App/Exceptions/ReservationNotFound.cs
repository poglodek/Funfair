using Funfair.Shared.App;

namespace Reservations.App.Exceptions;

public class ReservationNotFound(string value) : AppException(value)
{
    public override string ErrorMessage => "reservation_not_found";
}