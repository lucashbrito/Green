using Green.Application.Abstractions.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Shared;

namespace Green.Application.ChargeStation.Commands;

public record CreateChargeStationCommand(Guid GroupId, string Name) : ICommand<Domain.Entities.ChargeStation>;

public class CreateChargeStationCommandHandler : ICommandHandler<CreateChargeStationCommand, Domain.Entities.ChargeStation>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateChargeStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<Domain.Entities.ChargeStation>> Handle(CreateChargeStationCommand request, CancellationToken cancellationToken)
    {
        var group = await _unitOfWork.GroupRepository.GetById(request.GroupId);

        if (group is null)
            return Result<Domain.Entities.ChargeStation>.Failure($"Group with id {request.GroupId} not found");

        var chargeStation = new Domain.Entities.ChargeStation(request.Name, group);

        _unitOfWork.ChargeStationRepository.Add(chargeStation);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Domain.Entities.ChargeStation>.Success(chargeStation);
    }
}
