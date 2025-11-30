using Foodshare.Application.Dishes.Commands.CreateDish;
using Foodshare.Application.Dishes.Commands.DeleteDish;
using Foodshare.Application.Dishes.Commands.UpdateDish;
using Foodshare.Application.Dishes.Queries;
using Foodshare.Application.Dishes.Queries.GetDishById;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        
        group.MapGet("/", async (
            [FromQuery] string? searchTerm,
            [FromQuery] DishCategory? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] Guid? ownerId,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            IMediator mediator) =>
        {
            var query = new GetDishesQuery
            {
                SearchTerm = searchTerm,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                OwnerId = ownerId,
                PageNumber = pageNumber == 0 ? 1 : pageNumber,
                PageSize = pageSize == 0 ? 10 : pageSize
            };

            var result = await mediator.Send(query);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetDishByIdQuery(id));

            return result.Succeeded
                ? Results.Ok(result)
                : Results.NotFound(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateDishCommand command, IMediator mediator) =>
        {
            // Ensure the ID from route matches the command
            var updatedCommand = command with { Id = id };
            var result = await mediator.Send(updatedCommand);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, Guid requestingUserId, IMediator mediator) =>
        {
            var command = new DeleteDishCommand
            {
                Id = id,
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