using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Green.Infrastructure.Repositories;

public class ConnectorRepository : IConnectorRepository
{
    private GreenDbContext _dbContext;

    public ConnectorRepository(GreenDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Connector connector)
        => _dbContext.Connectors.Add(connector);

    public async Task<List<Connector>> GetByChargeStationId(Guid id)
    => await _dbContext.Connectors.Where(c => c.ChargeStationId == id).ToListAsync();

    public async Task<Connector> GetById(Guid connectorId)
        => await _dbContext.Connectors.FirstOrDefaultAsync(c => c.Id == connectorId);

    public void Remove(Connector connector)
        => _dbContext.Connectors.Remove(connector);

    public void Update(Connector connector)
        => _dbContext.Connectors.Update(connector);
}
