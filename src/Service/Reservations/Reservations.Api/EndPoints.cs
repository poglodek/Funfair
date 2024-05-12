using MediatR;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Commands.CreateReservation;
using Reservations.App.Commands.UpdateDraft;

namespace Reservations.Api;

public static class EndPoints
{
    public static void Map( IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("draft", async (CreateDraftCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return Results.Ok(result);
        });
        
        //TODO use one endpoint for many updates commands
        endpoints.MapPut("draft", async (UpdateDateDraftCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);

            return Results.Accepted();
        });
        
        endpoints.MapPost("reservation", async (CreateReservationCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return Results.Ok(result);
        });
    }
}