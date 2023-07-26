using Green.Application.Events;
using Green.Application.Services;
using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Green.Application;

public static class Configuration
{
    public static void SetApplications(this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IChargeStationService, ChargeStationService>();
        services.AddScoped<IConnectorService, ConnectorService>();
        services.AddScoped<INotificationHandler<ChargeStationRemovedDomainEvent>, ChargeStationRemovedDomainEventHandler>();
        services.AddScoped<INotificationHandler<GroupRemovedDomainEvent>, GroupRemovedDomainEventHandler>();
    }
}
