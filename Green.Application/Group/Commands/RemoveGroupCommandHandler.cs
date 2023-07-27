using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Group.Commands;
public record RemoveGroupCommand(Guid GroupId) : IRequest;
public class RemoveGroupCommandHandler : IRequestHandler<RemoveGroupCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveGroupCommandHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetById(request.GroupId);
        
        group.NullGuard("Group not found", nameof(group));

        group.RemoveChargeStations();

        _groupRepository.Remove(group);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
