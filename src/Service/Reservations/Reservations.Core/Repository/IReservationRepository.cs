using Reservations.Core.Entities;

namespace Reservations.Core.Repository;

public interface IReservationRepository
{
    Task Create(ReservationDraft reservationDraft, CancellationToken cancellationToken = default);
    Task<ReservationDraft> GetDraftById(Guid id, CancellationToken cancellationToken = default);
    Task Update(ReservationDraft draft, CancellationToken cancellationToken);
}