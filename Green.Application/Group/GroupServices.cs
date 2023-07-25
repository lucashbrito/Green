using Green.Domain.Abstractions;

namespace Green.Application.Group;

public class GroupService : IGroupService
{
    private IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    //option 1
    public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    //option 2
    public GroupService(IGroupRepository groupRepository, IChargeStationRepository chargeStationRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _chargeStationRepository = chargeStationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Domain.Group> CreateGroup(string name, int capacityInAmps)
    {
        var group = new Domain.Group(name, capacityInAmps);

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

    //Option 1
    public async Task RemoveGroup(Guid groupId)
    {
        var group = await _groupRepository.GetById(groupId) ?? throw new Exception("Group not found");

        group.RemoveChargeStations();

        _groupRepository.Remove(group);

        await _unitOfWork.CompleteAsync();
    }

    //Option 2
    private IChargeStationRepository _chargeStationRepository;
    public async Task RemoveGroup2(Guid groupId)
    {
        var group = await _groupRepository.GetById(groupId) ?? throw new Exception("Group not found");

        var chargeStations = await _chargeStationRepository.GetByGroupId(groupId);

        foreach (var chargeStation in chargeStations)
        {
            _chargeStationRepository.Remove(chargeStation);
        }

        _groupRepository.Remove(group);

        await _unitOfWork.CompleteAsync();
    }
}
