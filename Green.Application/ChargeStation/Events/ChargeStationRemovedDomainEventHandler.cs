using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;

namespace Green.Application.ChargeStation.Events;

public class ChargeStationRemovedDomainEventHandler : INotificationHandler<ChargeStationRemovedDomainEvent>
{
    IConnectorService _connectorService;

    public ChargeStationRemovedDomainEventHandler(IConnectorService connectorService)
    {
        _connectorService = connectorService;
    }

    public async Task Handle(ChargeStationRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _connectorService.RemoveConnectorByChargeStation(notification.ChargeStationId);
    }
}
