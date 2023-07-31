using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Green.Infrastructure.Repositories;

public class CachedConnectorRepository : IConnectorRepository
{
    private readonly IConnectorRepository _decorated;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan time = TimeSpan.FromMinutes(5);
    private readonly string cacheName = "connector";

    public CachedConnectorRepository(IConnectorRepository connectorRepository, IMemoryCache cache)
    {
        _decorated = connectorRepository;
        _cache = cache;
    }


    public void Add(Connector connector)
    {
        _decorated.Add(connector);
        _cache.Set($"{cacheName}-{connector.Id}", connector, time);
        _cache.Remove($"all-{cacheName}s");
    }

    public async Task<List<Connector>> GetAll()
    {
        var key = $"all-{cacheName}s";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetAll();
        });
    }

    public async Task<List<Connector>> GetByChargeStationId(Guid id)
    {
        var key = $"{cacheName}-ChargeStationId-{id}";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetByChargeStationId(id);
        });
    }

    public async Task<Connector> GetById(Guid connectorId)
    {
        var key = $"{cacheName}-{connectorId}";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetById(connectorId);
        });
    }

    public void Remove(Connector connector)
    {
        _decorated.Remove(connector);
        _cache.Remove($"{cacheName}-{connector.Id}");
    }

    public void Update(Connector connector)
    {
        _decorated.Update(connector);
        _cache.Set($"{cacheName}-{connector.Id}", connector, time);
    }
}
