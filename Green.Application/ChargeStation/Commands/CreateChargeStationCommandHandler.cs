using Green.Application.Abstractions.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Shared;

namespace Green.Application.ChargeStation.Commands;

public record CreateChargeStationCommand(Guid GroupId, string Name) : ICommand<Domain.Entities.ChargeStation>;

public class CreateChargeStationCommandHandler : ICommandHandler<CreateChargeStationCommand, Domain.Entities.ChargeStation>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IChargeStationRepository _chargeStationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateChargeStationCommandHandler(IGroupRepository groupRepository,
        IChargeStationRepository chargeStationRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _chargeStationRepository = chargeStationRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<Domain.Entities.ChargeStation>> Handle(CreateChargeStationCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetById(request.GroupId);

        if (group is null)
            return Result<Domain.Entities.ChargeStation>.Failure($"Group with id {request.GroupId} not found");

        var chargeStation = new Domain.Entities.ChargeStation(request.Name, group);

        _chargeStationRepository.Add(chargeStation);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result<Domain.Entities.ChargeStation>.Success(chargeStation);
    }
}
