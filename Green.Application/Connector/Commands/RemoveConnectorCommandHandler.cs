using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands;

public record RemoveConnectorCommand(Guid ConnectorId) : IRequest;

public class RemoveConnectorCommandHandler : IRequestHandler<RemoveConnectorCommand>
{
    private readonly IConnectorRepository _connectorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveConnectorCommandHandler(IConnectorRepository connectorRepository, IUnitOfWork unitOfWork)
    {
        _connectorRepository = connectorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveConnectorCommand request, CancellationToken cancellationToken)
    {
        var connector = await _connectorRepository.GetById(request.ConnectorId);

        connector.NullGuard("connector not found", nameof(connector));

        _connectorRepository.Remove(connector);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
