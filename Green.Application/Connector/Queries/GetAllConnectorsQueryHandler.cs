using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Connector.Queries;

public class GetAllConnectorsQuery : IRequest<List<Domain.Entities.Connector>> { }

public class GetAllConnectorsQueryHandler : IRequestHandler<GetAllConnectorsQuery, List<Domain.Entities.Connector>>
{
    private readonly IConnectorRepository _connectorRepository;

    public GetAllConnectorsQueryHandler(IConnectorRepository connectorRepository)
    {
        _connectorRepository = connectorRepository;
    }

    public async Task<List<Domain.Entities.Connector>> Handle(GetAllConnectorsQuery request, CancellationToken cancellationToken)
    {
        return await _connectorRepository.GetAll();
    }
}
