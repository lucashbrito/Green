using Green.Domain.Abstractions;
using Green.Domain.Extensions;
using MediatR;

namespace Green.Application.Connector.Commands
{
    public record CreateConnectorCommand(Guid StationId, int Identifier, int MaxCurrentInAmps) : IRequest<Domain.Entities.Connector>;

    public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, Domain.Entities.Connector>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateConnectorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.Entities.Connector> Handle(CreateConnectorCommand request, CancellationToken cancellationToken)
        {
            var station = await _unitOfWork.ChargeStationRepository.GetById(request.StationId);

            station.NullGuard("station not found", nameof(station));

            var connector = new Domain.Entities.Connector(request.Identifier, request.MaxCurrentInAmps, station.Id);

            if (!await CheckGroupCapacity(station.GroupId))
                throw new InvalidOperationException("The group's capacity is not sufficient.");

            _unitOfWork.ConnectorRepository.Add(connector);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return connector;
        }

        private async Task<bool> CheckGroupCapacity(Guid groupId)
        {
            var group = await _unitOfWork.GroupRepository.GetById(groupId);

            group.NullGuard("Group not found", nameof(group));

            var chargeStations = await _unitOfWork.ChargeStationRepository.GetByGroupId(groupId);
            var totalConnectorCurrent = 0.0;

            if (chargeStations is null)
                return group.HasSufficientGroupCapacity(totalConnectorCurrent);

            foreach (var station in chargeStations)
            {
                var connectors = await _unitOfWork.ConnectorRepository.GetByChargeStationId(station.Id);
                totalConnectorCurrent += connectors.Sum(c => c.MaxCurrentInAmps);
            }

            return group.HasSufficientGroupCapacity(totalConnectorCurrent);
        }
    }

}
