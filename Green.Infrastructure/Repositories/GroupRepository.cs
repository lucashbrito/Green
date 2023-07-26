using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Green.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private GreenDbContext _dbContext;

    public GroupRepository(GreenDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(Group group)
        => _dbContext.Groups.Add(group);

    public async Task<Group> GetById(Guid groupId)
        => await _dbContext.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

    public void Remove(Group group)
        => _dbContext.Groups.Remove(group);

    public void Update(Group group)
        => _dbContext.Groups.Update(group);
}
