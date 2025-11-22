using Foodshare.Application.Dishes.Commands.CreateDish;
using Foodshare.Application.Dishes.Queries;
using MediatR;

namespace Foodshare.API.Endpoints;

public static class DishEndpoints
{
    public static IEndpointRouteBuilder MapDishEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dishes")
            .WithTags("Dishes");

        group.MapPost("/", async (CreateDishCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result);
            }

            return Results.Created($"/api/dishes/{result.Data}", new { id = result.Data });
        });
        
        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDishesQuery());

            return result.Succeeded
                ? Results.Ok(new { Dishes = result.Data })
                : Results.BadRequest(result);
        });

        return app;
    }
}