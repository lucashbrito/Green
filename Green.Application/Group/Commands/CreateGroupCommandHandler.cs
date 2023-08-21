using Green.Domain.Abstractions;
using MediatR;

namespace Green.Application.Group.Commands;

public record CreateGroupCommand(string Name, int CapacityInAmps) : IRequest<Domain.Entities.Group>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Domain.Entities.Group>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Domain.Entities.Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new Domain.Entities.Group(request.Name, request.CapacityInAmps);

        _unitOfWork.GroupRepository.Add(group);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return group;
    }
}