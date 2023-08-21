using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Green.Infrastructure;

public static class Configuration
{

    public static void SetRepositories(this IServiceCollection services, string greenDbConnectionString)
    {
        services.AddMemoryCache();

        services.AddScoped<IChargeStationRepository, ChargeStationRepository>();
        services.AddScoped<IConnectorRepository, ConnectorRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Decorate<IChargeStationRepository, CachedChargeStationRepository>();
        services.Decorate<IConnectorRepository, CachedConnectorRepository>();
        services.Decorate<IGroupRepository, CachedGroupRepository>();

        services.AddDbContext<GreenDbContext>(options => options.UseSqlite("DataSource=../Green.Infrastructure/Green.db"));
    }
}
