using Funfair.Shared.App;

namespace Reservations.App.Exceptions;

public class SeatNotFound(string value) : AppException(value)
{
    public override string ErrorMessage => "seat_not_found";
}