using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IServices;

public interface IGroupService
{
    Task<Group> CreateGroup(string name, int capacityInAmps);
    Task UpdateGroup(Guid groupId, string name, int capacityInAmps);
    Task RemoveGroup(Guid groupId);
    Task<List<Group>> GetAll();
}
