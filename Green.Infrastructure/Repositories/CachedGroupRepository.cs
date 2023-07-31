using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Green.Infrastructure.Repositories;

public class CachedGroupRepository : IGroupRepository
{
    private readonly IGroupRepository _decorated;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan time = TimeSpan.FromMinutes(5);
    private readonly string cacheName = "group";
    public CachedGroupRepository(IGroupRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }


    public void Add(Group group)
    {
        _decorated.Add(group);
        _cache.Set($"{cacheName} -{group.Id}", group, time);
        _cache.Remove($"all-{cacheName}s");
    }

    public async Task<List<Group>> GetAll()
    {
        var key = $"all-{cacheName}s";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetAll();
        });
    }

    public async Task<Group> GetById(Guid groupId)
    {
        var key = $"{cacheName}-groupId-{groupId}";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(time);
            return await _decorated.GetById(groupId);
        });
    }

    public void Remove(Group group)
    {
        _decorated.Remove(group);
        _cache.Remove($"{cacheName}-{group.Id}");
    }

    public void Update(Group group)
    {
        _decorated.Update(group);
        _cache.Set($"{cacheName}-{group.Id}", group, time);
    }
}
