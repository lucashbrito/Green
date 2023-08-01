using Green.Application.Abstractions.Queries;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Shared;

namespace Green.Application.ChargeStation.Queries;

public class GetAllChargeStationsQuery : IQuery<List<Domain.Entities.ChargeStation>> { }

public class GetAllChargeStationsQueryHandler : IQueryHandler<GetAllChargeStationsQuery, List<Domain.Entities.ChargeStation>>
{
    private readonly IChargeStationRepository _chargeStationRepository;

    public GetAllChargeStationsQueryHandler(IChargeStationRepository chargeStationRepository)
    {
        _chargeStationRepository = chargeStationRepository;
    }

    public async Task<Result<List<Domain.Entities.ChargeStation>>> Handle(GetAllChargeStationsQuery request, CancellationToken cancellationToken)
    {
        return Result<List<Domain.Entities.ChargeStation>>.Success(await _chargeStationRepository.GetAll());
    }
}
