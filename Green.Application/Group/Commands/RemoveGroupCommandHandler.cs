using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Group.Commands;
public record RemoveGroupCommand(Guid GroupId) : IRequest;
public class RemoveGroupCommandHandler : IRequestHandler<RemoveGroupCommand>
{    
    private readonly IUnitOfWork _unitOfWork;

    public RemoveGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.GroupRepository.GetById(request.GroupId);
        
        group.NullGuard("Group not found", nameof(group));

        group.RemoveChargeStations();

        _unitOfWork.GroupRepository.Remove(group);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
