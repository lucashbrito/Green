namespace Green.Domain.Abstractions.IServices;

public interface IGroupService
{
    Task<bool> CheckGroupCapacity(Guid groupId, int maxCurrentInAmps);
}
