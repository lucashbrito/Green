using Green.Application.Services;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;
using Green.Domain.Entities;

namespace Green.Tests.Services;
 
public class ChargeStationServiceTests
{
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly Mock<IChargeStationRepository> _chargeStationRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ChargeStationService _service;

    public ChargeStationServiceTests()
    {
        _groupRepository = new Mock<IGroupRepository>();
        _chargeStationRepository = new Mock<IChargeStationRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _service = new ChargeStationService(_groupRepository.Object, _chargeStationRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task CreateChargeStation_ShouldCreateAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Group(groupName, 100);

        _groupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);

        // Act
        var chargeStation = await _service.CreateChargeStation(groupId, "Test ChargeStation");

        // Assert
        Assert.NotNull(chargeStation);
        Assert.Equal("Test ChargeStation", chargeStation.Name);
        _groupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.Add(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }


    [Fact]
    public async Task UpdateChargeStation_ShouldUpdateAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var stationName = "Test Station";
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Group(groupName, 100);
        var chargeStation = new ChargeStation(stationName, group);

        _chargeStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);
        _groupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);
        _chargeStationRepository.Setup(x => x.HasChargeStationInAnyGroupId(groupId)).ReturnsAsync(false);

        // Act
        await _service.UpdateChargeStation(stationId, "New Station Name", groupId);

        // Assert
        Assert.Equal("New Station Name", chargeStation.Name);
        _chargeStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _groupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.HasChargeStationInAnyGroupId(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.Update(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }


    [Fact]
    public async Task RemoveChargeStation_ShouldRemoveAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var stationName = "Test Station";
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Group(groupName, 100);
        var chargeStation = new ChargeStation(stationName, group);

        _chargeStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);

        // Act
        await _service.RemoveChargeStation(stationId);

        // Assert
        _chargeStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _chargeStationRepository.Verify(x => x.Remove(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }
}