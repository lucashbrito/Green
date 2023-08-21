using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands;

public record UpdateConnectorMaxCurrentCommand(Guid ConnectorId, int MaxCurrentInAmps) : IRequest;

public class UpdateConnectorMaxCurrentCommandHandler : IRequestHandler<UpdateConnectorMaxCurrentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConnectorMaxCurrentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateConnectorMaxCurrentCommand request, CancellationToken cancellationToken)
    {
        var connector = await _unitOfWork.ConnectorRepository.GetById(request.ConnectorId);

        connector.NullGuard("connector not found", nameof(connector));

        connector.ChangeMaxCurrent(request.MaxCurrentInAmps);

        _unitOfWork.ConnectorRepository.Update(connector);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}

