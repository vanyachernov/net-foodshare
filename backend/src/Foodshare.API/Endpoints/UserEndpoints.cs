using Foodshare.Application.Common.Users.Register;
using MediatR;

namespace Foodshare.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");
        
        group.MapPost("/", async (RegisterUserCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result);
            }

            return Results.Created($"/api/users", new { id = result.Data });
        });

        return app;
    }
}