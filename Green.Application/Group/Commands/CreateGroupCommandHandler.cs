using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Group.Commands;

public record CreateGroupCommand(string Name, int CapacityInAmps) : IRequest<Domain.Entities.Group>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Domain.Entities.Group>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Domain.Entities.Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new Domain.Entities.Group(request.Name, request.CapacityInAmps);

        _groupRepository.Add(group);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return group;
    }
}