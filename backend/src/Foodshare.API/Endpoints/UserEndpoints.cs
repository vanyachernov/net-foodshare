using Foodshare.Application.Common.Users.Commands.UpdateProfile;
using Foodshare.Application.Common.Users.Login;
using Foodshare.Application.Common.Users.Queries.GetUserById;
using Foodshare.Application.Common.Users.Queries.GetUserDishes;
using Foodshare.Application.Common.Users.Register;
using MediatR;

namespace Foodshare.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        group.MapPost("/register", async (RegisterUserCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result);
            }

            return Results.Created($"/api/users/{result.Data}", result);
        });

        group.MapPost("/login", async (LoginUserCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserByIdQuery(id));

            return result.Succeeded
                ? Results.Ok(result)
                : Results.NotFound(result);
        });

        group.MapGet("/{id:guid}/dishes", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserDishesQuery(id));

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateUserProfileCommand command, IMediator mediator) =>
        {
            var updatedCommand = command with { UserId = id };
            var result = await mediator.Send(updatedCommand);

            return result.Succeeded
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        return app;
    }
}