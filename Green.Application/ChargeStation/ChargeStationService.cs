using Green.Domain.Abstractions;

namespace Green.Application.ChargeStation;

public class ChargeStationService : IChargeStationService
{
    private IGroupRepository _groupRepository;
    private IChargeStationRepository _chargeStationRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ChargeStationService(IGroupRepository groupRepository, IChargeStationRepository chargeStationRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _chargeStationRepository = chargeStationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Domain.ChargeStation> CreateChargeStation(Guid groupId, string name)
    {
        var group = await _groupRepository.GetById(groupId);

        var chargeStation = new Domain.ChargeStation(name, group);

        group.AddChargeStation(chargeStation);

        _chargeStationRepository.Add(chargeStation);

        await _unitOfWork.CompleteAsync();

        return chargeStation;
    }

    public async Task UpdateChargeStation(Guid stationId, string name, Guid groupId)
    {
        var group = await _groupRepository.GetById(groupId);

        var chargeStation = await _chargeStationRepository.GetById(stationId);

        chargeStation.ChangeName(name);

        if (!await _chargeStationRepository.HasChargeStationInAnyGroupId(groupId))
            chargeStation.SetGroup(group);

        _chargeStationRepository.Update(chargeStation);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveChargeStation(Guid groupId, Guid stationId)
    {
        var group = _groupRepository.GetById(groupId);

        var station = _chargeStationRepository.GetById(stationId);

        //group.RemoveChargeStation(station);

        //_chargeStationRepository.Remove(station);

        await _unitOfWork.CompleteAsync();
    }
}
