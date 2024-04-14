using System.Linq.Expressions;
using Funfair.Shared.Core;
using Reservations.Core.Entities;

namespace Reservations.Core.Specification;

public class SpecificationDue(IClock clock) : SpecificationBase<Reservation>
{
    private const int MaxTimeBeforeFlightToEditReservation = -12;
    
    // Date now 01.01.2022 01:00
    // FlightDate 13.01.2022 01:00
    // 01.01.2022 01:00 < 13.01.2022 01:00 - 12
    // 01.01.2022 01:00 < 12.01.2022 13:00
    // return true
    protected override Expression<Func<Reservation, bool>> AsPredicateExpression()
    {
        return reservation => clock.CurrentDateTime <
                              reservation.FlightDate.Departure.AddHours(MaxTimeBeforeFlightToEditReservation);
    }
}