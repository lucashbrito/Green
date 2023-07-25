using Green.Domain.Abstractions;
using Green.Domain.DomainEvents;
using MediatR;

namespace Green.Application.Events;

public class GroupRemovedDomainEventHandler : INotificationHandler<GroupRemovedDomainEvent>
{
    IChargeStationRepository _chargeStationRepository;
    IConnectorRepository _connectorRepository;
    public GroupRemovedDomainEventHandler(IChargeStationRepository chargeStationRepository, IConnectorRepository connectorRepository)
    {
        _chargeStationRepository = chargeStationRepository;
        _connectorRepository = connectorRepository;
    }
    public async Task Handle(GroupRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var chargeStations = await _chargeStationRepository.GetByGroupId(notification.GroupId);

        if (chargeStations is null)
            return;

        foreach (var station in chargeStations)
        {
            Domain.Connector connect = await _connectorRepository.GetByChargeStationId(station.Id);

            if (connect is not null)
                _connectorRepository.Remove(connect);

            _chargeStationRepository.Remove(station);
        }

        await _chargeStationRepository.SaveChangesAsync();
        await _connectorRepository.SaveChangesAsync();
    }
}
