namespace TafelsStampen.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dataDirectory)
    {
        services.AddSingleton(new FilePathProvider(dataDirectory));
        services.AddScoped<IPlayerRepository, JsonPlayerRepository>();
        services.AddScoped<IGameSessionRepository, JsonGameSessionRepository>();
        services.AddScoped<IHallOfFameRepository, JsonHallOfFameRepository>();
        return services;
    }
}
