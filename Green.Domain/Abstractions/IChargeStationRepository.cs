namespace Green.Domain.Abstractions;

public interface IChargeStationRepository
{
    void Add(ChargeStation chargeStation);
    Task<List<ChargeStation>> GetByGroupId(Guid groupId);
    Task<ChargeStation> GetById(Guid stationId);
    Task<bool> HasChargeStationInAnyGroupId(Guid groupId);
    void Remove(ChargeStation station);
    Task SaveChangesAsync();
    void Update(ChargeStation chargeStation);
}
