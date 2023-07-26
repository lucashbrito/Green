using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions;
using Green.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Green.Infrastructure
{
    public static class Configuration
    {

        public static void SetRepositories(this IServiceCollection services, string greenDbConnectionString)
        {
            services.AddScoped<IChargeStationRepository, ChargeStationRepository>();
            services.AddScoped<IConnectorRepository, ConnectorRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<GreenDbContext>(options => options.UseInMemoryDatabase("GreenDbContext"));
        }
    }
}
