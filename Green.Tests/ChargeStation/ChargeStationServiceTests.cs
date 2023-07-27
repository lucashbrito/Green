using Green.Application.ChargeStation.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;

namespace Green.Tests.ChargeStation;

public class ChargeStationServiceTests
{
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly Mock<IChargeStationRepository> _chargeStationRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public ChargeStationServiceTests()
    {
        _groupRepository = new Mock<IGroupRepository>();
        _chargeStationRepository = new Mock<IChargeStationRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task CreateChargeStation_ShouldCreateAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Domain.Entities.Group(groupName, 100);
        var createChargeStationCommand = new CreateChargeStationCommand(groupId, "Test ChargeStation");
        var handler = new CreateChargeStationCommandHandler(_groupRepository.Object, _chargeStationRepository.Object, _unitOfWork.Object);

        _groupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);

        // Act
        var chargeStation = await handler.Handle(createChargeStationCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(chargeStation);
        Assert.Equal("Test ChargeStation", chargeStation.Name);
        _groupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.Add(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task UpdateChargeStation_ShouldUpdateAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var stationName = "Test Station";
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Domain.Entities.Group(groupName, 100);
        var chargeStation = new Domain.Entities.ChargeStation(stationName, group);

        var updateChargeStationCommand = new UpdateChargeStationCommand(stationId, "New Station Name", groupId);
        var handler = new UpdateChargeStationCommandHandler(_chargeStationRepository.Object, _groupRepository.Object, _unitOfWork.Object);

        _chargeStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);
        _groupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);
        _chargeStationRepository.Setup(x => x.HasChargeStationInAnyGroupId(groupId)).ReturnsAsync(false);

        // Act
        await handler.Handle(updateChargeStationCommand, CancellationToken.None);

        // Assert
        Assert.Equal("New Station Name", chargeStation.Name);
        _chargeStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _groupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.HasChargeStationInAnyGroupId(groupId), Times.Once);
        _chargeStationRepository.Verify(x => x.Update(It.IsAny<Domain.Entities.ChargeStation>()), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task RemoveChargeStation_ShouldRemoveAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var stationName = "Test Station";
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Domain.Entities.Group(groupName, 100);
        var chargeStation = new Domain.Entities.ChargeStation(stationName, group);

        var removeChargeStationCommand = new RemoveChargeStationCommand(stationId);
        var handler = new RemoveChargeStationCommandHandler(_chargeStationRepository.Object, _unitOfWork.Object);

        _chargeStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);

        // Act
        await handler.Handle(removeChargeStationCommand, CancellationToken.None);

        // Assert
        _chargeStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _chargeStationRepository.Verify(x => x.Remove(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}