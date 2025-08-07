using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using Shared.Infrastructure.Extensions;


namespace Shared.Infrastructure.DependencyInjection;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddSharedInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IHttpClientExtensions, HttpClientExtensions>();

        return services;
    }
}

