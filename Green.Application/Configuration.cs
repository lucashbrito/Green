using Green.Application.ChargeStation.Events;
using Green.Application.Connector;
using Green.Application.Group;
using Green.Application.Group.Events;
using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Green.Application;

public static class Configuration
{
    public static void AddApplications(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IConnectorService, ConnectorService>();
        services.AddScoped<IGroupService, GroupService>();

        services.AddScoped<INotificationHandler<ChargeStationRemovedDomainEvent>, ChargeStationRemovedDomainEventHandler>();
        services.AddScoped<INotificationHandler<GroupRemovedDomainEvent>, GroupRemovedDomainEventHandler>();
    }
}
