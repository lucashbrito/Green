namespace Green.Domain.Abstractions.IServices;

public interface IGroupServices
{
    Task<bool> CheckGroupCapacity(Guid groupId, int maxCurrentInAmps);
}
