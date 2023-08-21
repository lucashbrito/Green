using Green.Application.Abstractions.Queries;
using Green.Domain.Abstractions;
using Green.Domain.Shared;

namespace Green.Application.ChargeStation.Queries;

public class GetAllChargeStationsQuery : IQuery<List<Domain.Entities.ChargeStation>> { }

public class GetAllChargeStationsQueryHandler : IQueryHandler<GetAllChargeStationsQuery, List<Domain.Entities.ChargeStation>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllChargeStationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<Domain.Entities.ChargeStation>>> Handle(GetAllChargeStationsQuery request, CancellationToken cancellationToken)
    {
        return Result<List<Domain.Entities.ChargeStation>>.Success(await _unitOfWork.ChargeStationRepository.GetAll());
    }
}
