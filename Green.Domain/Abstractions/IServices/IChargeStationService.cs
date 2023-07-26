using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IServices
{
    public interface IChargeStationService
    {
        Task<ChargeStation> CreateChargeStation(Guid groupId, string name);
        Task UpdateChargeStation(Guid stationId, string name, Guid groupId);
        Task RemoveChargeStation(Guid stationId);
    }
}
