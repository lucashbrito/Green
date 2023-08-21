using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Extensions;

namespace Green.Application.Group
{
    public class GroupServices : IGroupServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckGroupCapacity(Guid groupId, int maxCurrentInAmps)
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

            totalConnectorCurrent += maxCurrentInAmps;

            return group.HasSufficientGroupCapacity(totalConnectorCurrent);
        }
    }
}
