using Shared.Application;
using user.application.Commands.CreateUser;
using user.application.Commands.UpdateUser;
using user.application.DTOs;
using user.application.Mediator;
using user.application.Queries.GetUserById;
using user.infrastructure.DependencyInjection;

namespace user.api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IRequestHandler<CreateUserCommand, Guid>, CreateUserHandler>();
        services.AddScoped<IRequestHandler<UpdateUserCommand>, UpdateUserHandler>();
        services.AddScoped<IRequestHandler<GetUserByIdQuery, UserDto>, GetUserByIdHandler>();

        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructureServices(config); // from InfrastructureServicesRegistration
        return services;
    }
}
