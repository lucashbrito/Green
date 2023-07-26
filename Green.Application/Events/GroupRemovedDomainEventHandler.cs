using Green.Domain;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Green.Domain.DomainEvents;
using MediatR;

namespace Green.Application.Events;

public class GroupRemovedDomainEventHandler : INotificationHandler<GroupRemovedDomainEvent>
{
    IChargeStationRepository _chargeStationRepository;
    private readonly IUnitOfWork _unitOfWork;
    IConnectorService _connectorService;

    public GroupRemovedDomainEventHandler(IChargeStationRepository chargeStationRepository,
        IConnectorService connectorService,
        IUnitOfWork unitOfWork)
    {
        _chargeStationRepository = chargeStationRepository;
        _connectorService = connectorService;
        _unitOfWork = unitOfWork;
    }

    // quem vai executar o primeiro UnitOfWork ou o Repository?
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

        await _unitOfWork.CompleteAsync();
    }
}
