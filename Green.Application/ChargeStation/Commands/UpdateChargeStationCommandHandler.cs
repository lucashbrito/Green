using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.ChargeStation.Commands;

public record UpdateChargeStationCommand(Guid StationId, string Name, Guid GroupId) : IRequest;

public class UpdateChargeStationCommandHandler : IRequestHandler<UpdateChargeStationCommand>
{
    private readonly IChargeStationRepository _chargeStationRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateChargeStationCommandHandler(IChargeStationRepository chargeStationRepository,
        IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _chargeStationRepository = chargeStationRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateChargeStationCommand request, CancellationToken cancellationToken)
    {
        var chargeStation = await _chargeStationRepository.GetById(request.StationId);

        chargeStation.NullGuard("Charge station not found", nameof(request.StationId));

        chargeStation.ChangeName(request.Name);

        if (request.GroupId != Guid.Empty)
        {
            if (!await _chargeStationRepository.HasChargeStationInAnyGroupId(request.GroupId))
            {
                var group = await _groupRepository.GetById(request.GroupId);

                if (group is not null)
                    chargeStation.SetGroup(group);
            }
        }

        _chargeStationRepository.Update(chargeStation);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
