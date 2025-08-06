using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using user.application.Interfaces;
using user.infrastructure.Persistence.DbContexts;
using user.infrastructure.Persistence.Repositories;
using user.infrastructure.Services;


namespace user.infrastructure.DependencyInjection;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IDateTimeService, DateTimeService>();

        return services;
    }
}

