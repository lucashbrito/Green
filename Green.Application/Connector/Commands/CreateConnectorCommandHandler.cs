using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using MediatR;

namespace Green.Application.Connector.Commands
{
    public record CreateConnectorCommand(Guid StationId, int Identifier, int MaxCurrentInAmps) : IRequest<Domain.Entities.Connector>;

    public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, Domain.Entities.Connector>
    {
        private readonly IChargeStationRepository _chargeStationRepository;
        private readonly IConnectorRepository _connectorRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateConnectorCommandHandler(IChargeStationRepository stationRepository,
            IConnectorRepository connectorRepository,
            IUnitOfWork unitOfWork,
            IGroupRepository groupRepository)
        {
            _chargeStationRepository = stationRepository;
            _connectorRepository = connectorRepository;
            _unitOfWork = unitOfWork;
            _groupRepository = groupRepository;
        }

        public async Task<Domain.Entities.Connector> Handle(CreateConnectorCommand request, CancellationToken cancellationToken)
        {
            var station = await _chargeStationRepository.GetById(request.StationId);

            var connector = new Domain.Entities.Connector(request.Identifier, request.MaxCurrentInAmps, station);

            if (!await CheckGroupCapacity(station.GroupId))
                throw new InvalidOperationException("The group's capacity is not sufficient.");

            _connectorRepository.Add(connector);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return connector;
        }

        private async Task<bool> CheckGroupCapacity(Guid groupId)
        {
            var group = await _groupRepository.GetById(groupId);

            var chargeStations = await _chargeStationRepository.GetByGroupId(groupId);
            var totalConnectorCurrent = 0.0;

            foreach (var station in chargeStations)
            {
                var connectors = await _connectorRepository.GetByChargeStationId(station.Id);
                totalConnectorCurrent += connectors.Sum(c => c.MaxCurrentInAmps);
            }

            return group.HasSufficientGroupCapacity(totalConnectorCurrent);
        }
    }

}
