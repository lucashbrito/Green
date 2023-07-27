using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Group.Commands
{
    public record UpdateGroupCommand(Guid GroupId, string Name, int CapacityInAmps) : IRequest;

    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGroupCommandHandler(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetById(request.GroupId) ?? throw new Exception("Group not found");

            group.ChangeName(request.Name);
            group.ChangeCapacity(request.CapacityInAmps);

            _groupRepository.Update(group);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
