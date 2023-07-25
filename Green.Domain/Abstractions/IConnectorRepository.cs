namespace Green.Domain.Abstractions;

public interface IConnectorRepository
{
    void Add(Connector connector);
    Task<Connector> GetByChargeStationId(Guid id);
    Task<Connector> GetById(Guid connectorId);
    void Remove(Connector connector);
    Task SaveChangesAsync();
    void Update(Connector connector);
}
