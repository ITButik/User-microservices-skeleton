using MediatR;
using user.application.Commands.CreateUser;
using user.application.Commands.UpdateUser;
using user.application.Queries.GetUserById;

namespace user.api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/users", async (CreateUserCommand cmd, IMediator mediator) =>
        {
            var userId = await mediator.Send(cmd);
            return Results.Created($"/users/{userId}", userId);
        });

        routes.MapPut("/users/{id}", async (Guid id, UpdateUserCommand cmd, IMediator mediator) =>
        {
            await mediator.Send(cmd with { Id = id });
            return Results.NoContent();
        });

        routes.MapGet("/users/{id}", async (Guid id, IMediator mediator) =>
        {
            var user = await mediator.Send(new GetUserByIdQuery(id));
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        return routes;
    }
}
