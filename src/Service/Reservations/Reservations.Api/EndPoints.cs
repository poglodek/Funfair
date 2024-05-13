using MediatR;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Commands.CreateReservation;
using Reservations.App.Commands.UpdateDraft;
using Reservations.Infrastructure.Query.GetDraftById;
using Reservations.Infrastructure.Query.GetReservationById;
using Reservations.Infrastructure.Query.GetUserReservation;

namespace Reservations.Api;

public static class EndPoints
{
    public static IEndpointRouteBuilder Map( IEndpointRouteBuilder endpoints)
    {
        //POST
        endpoints.MapPost("draft", async (CreateDraftCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return Results.Ok(result);
        });
        
        
        endpoints.MapPost("reservation", async (CreateReservationCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return Results.Ok(result);
        });
        
        //PUT
        endpoints.MapPut("draft", async (UpdateDraftCommandBase command, IMediator mediator) =>
        {
            await mediator.Send(command);

            return Results.Accepted();
        });
        
        //GET
        endpoints.MapGet("draft/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDraftByIdCommand(id));

            return Results.Ok(result);
        });
        
        endpoints.MapGet("reservation/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetReservationByIdCommand(id));

            return Results.Ok(result);
        });
        
        endpoints.MapGet("reservations", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserReservationCommand());

            return Results.Ok(result);
        });


        return endpoints;
    }
}