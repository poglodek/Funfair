using Funfair.Dal.CosmosDb.Linq;
using Funfair.Dal.CosmosDb.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Reservations.Core.Entities;
using Reservations.Core.Repository;
using Reservations.Infrastructure.Dal.Container;

namespace Reservations.Infrastructure.Dal.Repositories;

public class ReservationRepository(IRepositoryBase<ReservationContainer> repositoryBase, ReservationContainer container) : IReservationRepository
{
    
    
    
    public Task Create(ReservationDraft reservationDraft, CancellationToken cancellationToken = default)
    {
        return repositoryBase.CreateItemAsync(reservationDraft, partitionKey: new PartitionKey(reservationDraft.Journey.Departure.IAtaCode), type: ReservationType.Draft ,cancellationToken: cancellationToken);
    }

    public Task<ReservationDraft?> GetDraftById(Guid id, CancellationToken cancellationToken = default)
    {
        return repositoryBase.GetById<ReservationDraft>(id, type: ReservationType.Draft, cancellationToken: cancellationToken);
    }

    public Task Update(ReservationDraft draft, CancellationToken cancellationToken)
    {
        return repositoryBase.UpsertItemAsync(draft, type: ReservationType.Draft, cancellationToken: cancellationToken);
    }

    public Task AddNewReservation(Reservation reservation, CancellationToken cancellationToken)
    {
        return repositoryBase.UpsertItemAsync(reservation, partitionKey: new PartitionKey(reservation.Journey.Departure.IAtaCode), type: ReservationType.Reservation, cancellationToken: cancellationToken);
        
    }

    public Task<Reservation?> GetById(Guid requestReservationId, CancellationToken cancellationToken)
    {
        return repositoryBase.GetById<Reservation>(requestReservationId, type: ReservationType.Reservation, cancellationToken: cancellationToken);
    }

    public Task Update(Reservation reservation, CancellationToken cancellationToken)
    {
        return repositoryBase.UpsertItemAsync(reservation, type: ReservationType.Reservation, cancellationToken: cancellationToken);
    }

    public Task<List<Reservation>> GetUserReservation(Guid requestUserId, CancellationToken cancellationToken)
    {
        return repositoryBase.GetItemLinqQueryable<Reservation>()
            .Where(x => x.UserReservations
                .Select(c => c.User.Id)
                .Contains(requestUserId))
            .ToFeedIterator()
            .ToListAsync(cancellationToken);
    }
}