using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Entities;

namespace Green.Application.Services;

public class GroupService : IGroupService
{
    private IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Group> CreateGroup(string name, int capacityInAmps)
    {
        var group = new Group(name, capacityInAmps);

        _groupRepository.Add(group);

        await _unitOfWork.CompleteAsync();

        return group;
    }

    public async Task UpdateGroup(Guid groupId, string name, int capacityInAmps)
    {
        var group = await _groupRepository.GetById(groupId) ?? throw new Exception("Group not found");

        group.ChangeName(name);
        group.ChangeCapacity(capacityInAmps);

        _groupRepository.Update(group);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveGroup(Guid groupId)
    {
        var group = await _groupRepository.GetById(groupId) ?? throw new Exception("Group not found");

        group.RemoveChargeStations();

        _groupRepository.Remove(group);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<List<Group>> GetAll()
    {
        return await _groupRepository.GetAll();
    }
}
