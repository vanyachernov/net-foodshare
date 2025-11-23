using Foodshare.Application.Reservations.Commands.CreateReservation;
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
        
        return app;
    }
}