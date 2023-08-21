using Green.Domain.Abstractions;
using MediatR;

namespace Green.Application.Group.Queries;

public class GetAllGroupsQuery : IRequest<List<Domain.Entities.Group>> { }

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<Domain.Entities.Group>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGroupsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Domain.Entities.Group>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GroupRepository.GetAll();
    }
}
