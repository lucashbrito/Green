using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.ChargeStation.Queries;

public class GetAllChargeStationsQuery : IRequest<List<Domain.Entities.ChargeStation>> { }

public class GetAllChargeStationsQueryHandler : IRequestHandler<GetAllChargeStationsQuery, List<Domain.Entities.ChargeStation>>
{
    private readonly IChargeStationRepository _chargeStationRepository;

    public GetAllChargeStationsQueryHandler(IChargeStationRepository chargeStationRepository)
    {
        _chargeStationRepository = chargeStationRepository;
    }

    public async Task<List<Domain.Entities.ChargeStation>> Handle(GetAllChargeStationsQuery request, CancellationToken cancellationToken)
    {
        return await _chargeStationRepository.GetAll();
    }
}
