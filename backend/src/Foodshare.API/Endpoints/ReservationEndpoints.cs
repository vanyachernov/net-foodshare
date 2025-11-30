using Foodshare.Application.Reservations.Commands.CancelReservation;
using Foodshare.Application.Reservations.Commands.CreateReservation;
using Foodshare.Application.Reservations.Commands.UpdateReservationStatus;
using Foodshare.Application.Reservations.Queries.GetDishReservations;
using Foodshare.Application.Reservations.Queries.GetUserReservations;
using MediatR;

namespace Foodshare.API.Endpoints;

public static class ReservationEndpoints
{
    public static IEndpointRouteBuilder MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations")
            .WithTags("Reservations");

        group.MapPost("/", async (CreateReservationCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result);
            }

            return Results.Created($"/api/reservations/{result.Data}", result);
        });

        group.MapGet("/user/{userId:guid}", async (Guid userId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserReservationsQuery(userId));

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapGet("/dish/{dishId:guid}", async (Guid dishId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDishReservationsQuery(dishId));

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapPut("/{id:guid}/status", async (Guid id, UpdateReservationStatusCommand command, IMediator mediator) =>
        {
            var updatedCommand = command with { ReservationId = id };
            var result = await mediator.Send(updatedCommand);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, Guid requestingUserId, IMediator mediator) =>
        {
            var command = new CancelReservationCommand
            {
                ReservationId = id,
                RequestingUserId = requestingUserId
            };

            var result = await mediator.Send(command);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });
        
        return app;
    }
}