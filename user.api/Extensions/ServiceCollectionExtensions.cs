using user.application.Commands.CreateUser;
using user.infrastructure.DependencyInjection;

namespace user.api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructureServices(config); // from InfrastructureServicesRegistration
        return services;
    }
}
