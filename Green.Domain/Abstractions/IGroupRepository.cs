namespace Green.Domain.Abstractions;

public interface IGroupRepository
{
    void Add(Group group);
    Task<Group> GetById(Guid groupId);
    void Remove(Group group);
    Task SaveChangesAysnc();
    void Update(Group group);
}
