using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;

namespace Green.Application.Group.Events;

public class GroupRemovedDomainEventHandler : INotificationHandler<GroupRemovedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConnectorService _connectorService;

    public GroupRemovedDomainEventHandler(IUnitOfWork unitOfWork, IConnectorService connectorService)
    {
        _unitOfWork = unitOfWork;
        _connectorService = connectorService;
    }

    public async Task Handle(GroupRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var chargeStations = await _unitOfWork.ChargeStationRepository.GetByGroupId(notification.GroupId);

        if (chargeStations is null)
            return;

        foreach (var station in chargeStations)
        {
            await _connectorService.RemoveConnectorByChargeStation(station.Id);

            _unitOfWork.ChargeStationRepository.Remove(station);
        }
    }
}
