using Funfair.Shared.Api.Hateos;
using MediatR;
using Reservations.Api.Requests;
using Reservations.App.Commands.CreateDraft;
using Reservations.App.Commands.CreateReservation;
using Reservations.App.Commands.CreateUserReservation;
using Reservations.App.Commands.UpdateDraft;
using Reservations.App.Dtos;
using Reservations.Infrastructure.Dtos;
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

            var resource = new Resource<CreateDraftCommandDto>(result, [
                new($"/draft/{result.Id}", "self", "GET"),
                new($"/reservation/{result.Id}", "create-reservation", "POST"),
                new($"/draft", "update-draft-reservation", "PUT")
            ]);
            
            return Results.Created($"/reservation/{result.Id}",resource);
        });
        
        
        endpoints.MapPost("reservation/{id}", async (CreateReservationRequest request, Guid id,IMediator mediator) =>
        {
            var command = new CreateReservationCommand(id, request.Price);
            
            var result = await mediator.Send(command);

            var resource = new Resource<ReservationIdDto>(result, [
                new($"/reservation/{id}", "self", "GET")
            ]);
            
            return Results.Ok(resource);
        });

        endpoints.MapPost("ReservationUser", async (CreateUserReservationCommand request, IMediator mediator) =>
        {
            await mediator.Send(request);
            var resource = new Resource<object>(null, [
                new Link($"/reservations", "self", "GET")
            ]);

            return Results.Ok(resource);
        });
        
        //PUT
        endpoints.MapPut("draft/{id}", async (UpdateDraftCommandBase command, Guid id,IMediator mediator) =>
        {
            await mediator.Send(command);
            
            var resource = new Resource<object>(null, [
                new($"/draft/{id}", "self", "GET"),
                new($"/reservation/{id}", "create-reservation", "POST"),
                new($"/draft", "update-draft-reservation", "PUT")
            ]);

            return Results.Ok(resource);
        });
        
        //GET
        endpoints.MapGet("draft/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDraftByIdCommand(id));
            
            var resource = new Resource<DraftDto>(result, [
                new($"/draft/{id}", "self", "GET"),
                new($"/reservation/{id}", "create-reservation", "POST"),
                new($"/draft", "update-draft-reservation", "PUT")
            ]);

            return Results.Ok(resource);
        });
        
        endpoints.MapGet("reservation/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetReservationByIdCommand(id));
            
            var resource = new Resource<ReservationDto>(result, [
                new($"/reservation/{id}", "self", "GET"),
            ]);


            return Results.Ok(resource);
        });
        
        endpoints.MapGet("reservations", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserReservationCommand());

            var resource = new Resource<UserReservationsDto>(result, [
                new($"/reservations", "self", "GET"),
            ]);
            
            return Results.Ok(resource);
        });
    

        return endpoints;
    }
}