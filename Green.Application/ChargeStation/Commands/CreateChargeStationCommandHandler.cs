using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.ChargeStation.Commands;

public record CreateChargeStationCommand(Guid GroupId, string Name) : IRequest<Domain.Entities.ChargeStation>;

public class CreateChargeStationCommandHandler : IRequestHandler<CreateChargeStationCommand, Domain.Entities.ChargeStation>
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

    public async Task<Domain.Entities.ChargeStation> Handle(CreateChargeStationCommand request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetById(request.GroupId);

        var chargeStation = new Domain.Entities.ChargeStation(request.Name, group);

        group.AddChargeStation(chargeStation);

        _chargeStationRepository.Add(chargeStation);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return chargeStation;
    }
}
