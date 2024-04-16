using Reservations.Core.Entities;

namespace Reservations.Core.Repository;

public interface IReservationRepository
{
    Task Create(ReservationDraft reservationDraft, CancellationToken cancellationToken = default);
}