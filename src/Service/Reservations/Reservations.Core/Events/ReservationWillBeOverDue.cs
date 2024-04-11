using Funfair.Shared.Exception;

namespace Reservations.Core.Events;

public class ReservationWillBeOverDue(Guid id) : CoreException($"Reservation draft with id {id} will be over due.")
{
    public override string ErrorMessage => "reservation_will_be_over_due";
}