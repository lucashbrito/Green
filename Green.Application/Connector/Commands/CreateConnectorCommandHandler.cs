using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands;

public record CreateConnectorCommand(Guid StationId, int Identifier, int MaxCurrentInAmps) : IRequest<Domain.Entities.Connector>;

public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, Domain.Entities.Connector>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGroupService _groupService;

    public CreateConnectorCommandHandler(IUnitOfWork unitOfWork, IGroupService groupSerivce)
    {
        _unitOfWork = unitOfWork;
        _groupService = groupSerivce;
    }

    public async Task<Domain.Entities.Connector> Handle(CreateConnectorCommand request, CancellationToken cancellationToken)
    {
        var station = await _unitOfWork.ChargeStationRepository.GetById(request.StationId);

        station.NullGuard("station not found", nameof(station));

        var connector = new Domain.Entities.Connector(request.Identifier, request.MaxCurrentInAmps, station.Id);

        if (!await _groupService.CheckGroupCapacity(station.GroupId, connector.MaxCurrentInAmps))
            throw new InvalidOperationException("The group's capacity is not sufficient.");

        _unitOfWork.ConnectorRepository.Add(connector);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return connector;
    }
}
