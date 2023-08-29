using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Group.Commands;

public record UpdateGroupCommand(Guid GroupId, string Name, int CapacityInAmps) : IRequest;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.GroupRepository.GetById(request.GroupId);

        group.NullGuard("Group not found", nameof(group));

        group.ChangeName(request.Name);
        group.ChangeCapacity(request.CapacityInAmps);

        _unitOfWork.GroupRepository.Update(group);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
