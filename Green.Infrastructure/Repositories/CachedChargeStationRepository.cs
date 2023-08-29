using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Green.Infrastructure.Repositories;

public class CachedChargeStationRepository : IChargeStationRepository
{
    private readonly IChargeStationRepository _decorated;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan time = TimeSpan.FromMinutes(5);
    private readonly string cacheName = "chargeStation";

    public CachedChargeStationRepository(IChargeStationRepository chargeStationRepository, IMemoryCache cache)
    {
        _decorated = chargeStationRepository;
        _cache = cache;
    }

    public void Add(ChargeStation chargeStation)
    {
        _decorated.Add(chargeStation);
        _cache.Set($"{cacheName}-{chargeStation.Id}", chargeStation, time);
        _cache.Remove($"all-{cacheName}s");
    }

    public async Task<List<ChargeStation>> GetAll()
    {
        var key = $"all-{cacheName}s";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetAll();
        });
    }

    public async Task<List<ChargeStation>> GetByGroupId(Guid groupId)
    {
        var key = $"chargeStation-{groupId}";


        return await _cache.GetOrCreateAsync(key,
            entry =>
            {
                entry.SetAbsoluteExpiration(time);

                return _decorated.GetByGroupId(groupId);
            });
    }

    public async Task<ChargeStation> GetById(Guid stationId)
    {
        return await _decorated.GetById(stationId);
    }

    public async Task<bool> HasChargeStationInAnyGroupId(Guid groupId)
    {
        return await _decorated.HasChargeStationInAnyGroupId(groupId);
    }

    public void Remove(ChargeStation chargeStation)
    {
        _decorated.Remove(chargeStation);
        _cache.Remove($"{cacheName}-{chargeStation.Id}");
    }

    public void Update(ChargeStation chargeStation)
    {
        _decorated.Update(chargeStation);
        _cache.Set($"{cacheName} -{chargeStation.Id}", chargeStation, time);
    }
}
