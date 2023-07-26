using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IRepositories;

public interface IConnectorRepository
{
    void Add(Connector connector);
    Task<List<Connector>> GetByChargeStationId(Guid id);
    Task<Connector> GetById(Guid connectorId);
    void Remove(Connector connector);
    void Update(Connector connector);
}
