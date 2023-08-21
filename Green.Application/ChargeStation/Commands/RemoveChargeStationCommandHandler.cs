using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.ChargeStation.Commands
{
    public record RemoveChargeStationCommand(Guid StationId) : IRequest;

    public class RemoveChargeStationCommandHandler : IRequestHandler<RemoveChargeStationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveChargeStationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveChargeStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _unitOfWork.ChargeStationRepository.GetById(request.StationId);

            station.NullGuard("Charge station not found", nameof(request.StationId));

            station.RemoveConnectors();

            _unitOfWork.ChargeStationRepository.Remove(station);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
