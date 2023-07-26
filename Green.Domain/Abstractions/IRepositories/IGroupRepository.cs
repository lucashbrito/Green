using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IRepositories;

public interface IGroupRepository
{
    void Add(Group group);
    Task<Group> GetById(Guid groupId);
    void Remove(Group group);
    void Update(Group group);
}
