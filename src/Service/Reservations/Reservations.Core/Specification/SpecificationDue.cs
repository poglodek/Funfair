using System.Linq.Expressions;
using Funfair.Shared.Core;
using Reservations.Core.Entities;

namespace Reservations.Core.Specification;

public class SpecificationDue(IClock clock) : SpecificationBase<Reservation>
{
    private const int MaxTimeBeforeFlightToEditReservation = -12;
    
    
    protected override Expression<Func<Reservation, bool>> AsPredicateExpression()
    {
        return reservation => clock.CurrentDateTime >
                              reservation.FlightDate.Departure.AddHours(MaxTimeBeforeFlightToEditReservation);
    }
}