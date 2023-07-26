using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IRepositories;

public interface IChargeStationRepository
{
    void Add(ChargeStation chargeStation);
    Task<List<ChargeStation>> GetByGroupId(Guid groupId);
    Task<ChargeStation> GetById(Guid stationId);
    Task<bool> HasChargeStationInAnyGroupId(Guid groupId);
    void Remove(ChargeStation station);
    void Update(ChargeStation chargeStation);
    Task<List<ChargeStation>> GetAll();
}
