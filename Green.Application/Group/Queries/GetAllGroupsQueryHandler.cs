using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Group.Queries;

public class GetAllGroupsQuery : IRequest<List<Domain.Entities.Group>> { }

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<Domain.Entities.Group>>
{
    private readonly IGroupRepository _groupRepository;

    public GetAllGroupsQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<List<Domain.Entities.Group>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _groupRepository.GetAll();
    }
}
