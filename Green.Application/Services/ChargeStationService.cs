using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Entities;

namespace Green.Application.Services;

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

    public async Task<ChargeStation> CreateChargeStation(Guid groupId, string name)
    {
        var group = await _groupRepository.GetById(groupId);

        var chargeStation = new ChargeStation(name, group);

        group.AddChargeStation(chargeStation);

        _chargeStationRepository.Add(chargeStation);

        await _unitOfWork.CompleteAsync();

        return chargeStation;
    }

    public async Task UpdateChargeStation(Guid stationId, string name, Guid groupId)
    {
        var chargeStation = await _chargeStationRepository.GetById(stationId);

        chargeStation.ChangeName(name);

        if (groupId != Guid.Empty)
        {
            var group = await _groupRepository.GetById(groupId);

            if (!await _chargeStationRepository.HasChargeStationInAnyGroupId(groupId))
                chargeStation.SetGroup(group);
        }

        _chargeStationRepository.Update(chargeStation);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveChargeStation(Guid stationId)
    {
        var station = await _chargeStationRepository.GetById(stationId);

        station.RemoveConnectors();

        _chargeStationRepository.Remove(station);

        await _unitOfWork.CompleteAsync();
    }
}
