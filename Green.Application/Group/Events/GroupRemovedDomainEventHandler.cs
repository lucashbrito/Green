using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;

namespace Green.Application.Group.Events;

public class GroupRemovedDomainEventHandler : INotificationHandler<GroupRemovedDomainEvent>
{
    private readonly IChargeStationRepository _chargeStationRepository;
    private readonly IConnectorService _connectorService;

    public GroupRemovedDomainEventHandler(IChargeStationRepository chargeStationRepository, IConnectorService connectorService)
    {
        _chargeStationRepository = chargeStationRepository;
        _connectorService = connectorService;
    }

    public async Task Handle(GroupRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var chargeStations = await _chargeStationRepository.GetByGroupId(notification.GroupId);

        if (chargeStations is null)
            return;

        foreach (var station in chargeStations)
        {
            await _connectorService.RemoveConnectorByChargeStation(station.Id);

            _chargeStationRepository.Remove(station);
        }
    }
}
