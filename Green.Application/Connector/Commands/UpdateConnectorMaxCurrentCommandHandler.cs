using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Connector.Commands;

public record UpdateConnectorMaxCurrentCommand(Guid ConnectorId, int MaxCurrentInAmps) : IRequest;

public class UpdateConnectorMaxCurrentCommandHandler : IRequestHandler<UpdateConnectorMaxCurrentCommand>
{
    private readonly IConnectorRepository _connectorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConnectorMaxCurrentCommandHandler(IConnectorRepository connectorRepository, IUnitOfWork unitOfWork)
    {
        _connectorRepository = connectorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateConnectorMaxCurrentCommand request, CancellationToken cancellationToken)
    {
        var connector = await _connectorRepository.GetById(request.ConnectorId);
        connector.ChangeMaxCurrent(request.MaxCurrentInAmps);

        _connectorRepository.Update(connector);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}

