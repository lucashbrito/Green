using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands;


public record RemoveConnectorCommand(Guid ConnectorId) : IRequest;

public class RemoveConnectorCommandHandler : IRequestHandler<RemoveConnectorCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveConnectorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveConnectorCommand request, CancellationToken cancellationToken)
    {
        var connector = await _unitOfWork.ConnectorRepository.GetById(request.ConnectorId);

        connector.NullGuard("connector not found", nameof(connector));

        _unitOfWork.ConnectorRepository.Remove(connector);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}