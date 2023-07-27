using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.ChargeStation.Commands
{
    public record RemoveChargeStationCommand(Guid StationId) : IRequest;

    public class RemoveChargeStationCommandHandler : IRequestHandler<RemoveChargeStationCommand>
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveChargeStationCommandHandler(IChargeStationRepository chargeStationRepository, IUnitOfWork unitOfWork)
        {
            _chargeStationRepository = chargeStationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveChargeStationCommand request, CancellationToken cancellationToken)
        {
            var station = await _chargeStationRepository.GetById(request.StationId) ?? throw new ArgumentException("Charge station not found", nameof(request.StationId));

            station.RemoveConnectors();

            _chargeStationRepository.Remove(station);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
