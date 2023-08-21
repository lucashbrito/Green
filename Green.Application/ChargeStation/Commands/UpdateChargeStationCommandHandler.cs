using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.ChargeStation.Commands;

public record UpdateChargeStationCommand(Guid StationId, string Name, Guid GroupId) : IRequest;

public class UpdateChargeStationCommandHandler : IRequestHandler<UpdateChargeStationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateChargeStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateChargeStationCommand request, CancellationToken cancellationToken)
    {
        var chargeStation = await _unitOfWork.ChargeStationRepository.GetById(request.StationId);

        chargeStation.NullGuard("Charge station not found", nameof(request.StationId));

        chargeStation.ChangeName(request.Name);

        if (request.GroupId != Guid.Empty)
        {
            if (!await _unitOfWork.ChargeStationRepository.HasChargeStationInAnyGroupId(request.GroupId))
            {
                var group = await _unitOfWork.GroupRepository.GetById(request.GroupId);

                if (group is not null)
                    chargeStation.SetGroup(group);
            }
        }

        _unitOfWork.ChargeStationRepository.Update(chargeStation);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
