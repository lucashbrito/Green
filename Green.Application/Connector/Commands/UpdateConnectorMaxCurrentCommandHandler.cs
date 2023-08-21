using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands;

public record UpdateConnectorMaxCurrentCommand(Guid ConnectorId, int MaxCurrentInAmps) : IRequest;

public class UpdateConnectorMaxCurrentCommandHandler : IRequestHandler<UpdateConnectorMaxCurrentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGroupServices _groupService;

    public UpdateConnectorMaxCurrentCommandHandler(IUnitOfWork unitOfWork, IGroupServices groupService)
    {
        _unitOfWork = unitOfWork;
        _groupService = groupService;
    }

    public async Task Handle(UpdateConnectorMaxCurrentCommand request, CancellationToken cancellationToken)
    {
        var connector = await _unitOfWork.ConnectorRepository.GetById(request.ConnectorId);

        connector.NullGuard("connector not found", nameof(connector));

        var station = await _unitOfWork.ChargeStationRepository.GetById(connector.ChargeStationId);

        station.NullGuard("station not found", nameof(station));

        if (!await _groupService.CheckGroupCapacity(station.GroupId, connector.MaxCurrentInAmps))
            throw new InvalidOperationException("The group's capacity is not sufficient.");

        connector.ChangeMaxCurrent(request.MaxCurrentInAmps);

        _unitOfWork.ConnectorRepository.Update(connector);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}

