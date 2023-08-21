using FluentAssertions;
using Green.Application.ChargeStation.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;

namespace Green.Tests.ChargeStation;

public class ChargeStationTests
{
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    private readonly Mock<IChargeStationRepository> _mockStationRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public ChargeStationTests()
    {
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockStationRepository = new Mock<IChargeStationRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _unitOfWork.Setup(u => u.ChargeStationRepository).Returns(_mockStationRepository.Object);
        _unitOfWork.Setup(u => u.GroupRepository).Returns(_mockGroupRepository.Object);
    }

    [Fact]
    public async Task CreateChargeStation_ShouldCreateAChargeStation_WhenGivenValidParameters()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var groupName = "Test Group";
        var group = new Domain.Entities.Group(groupName, 100);
        var createChargeStationCommand = new CreateChargeStationCommand(groupId, "Test ChargeStation");
        var handler = new CreateChargeStationCommandHandler(_unitOfWork.Object);

        _mockGroupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);

        // Act
        var result = await handler.Handle(createChargeStationCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test ChargeStation", result.Value.Name);
        _mockGroupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _mockStationRepository.Verify(x => x.Add(result.Value), Times.Once);
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
        var handler = new UpdateChargeStationCommandHandler(_unitOfWork.Object);

        _mockStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);
        _mockGroupRepository.Setup(x => x.GetById(groupId)).ReturnsAsync(group);
        _mockStationRepository.Setup(x => x.HasChargeStationInAnyGroupId(groupId)).ReturnsAsync(false);

        // Act
        await handler.Handle(updateChargeStationCommand, CancellationToken.None);

        // Assert
        Assert.Equal("New Station Name", chargeStation.Name);
        _mockStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _mockGroupRepository.Verify(x => x.GetById(groupId), Times.Once);
        _mockStationRepository.Verify(x => x.HasChargeStationInAnyGroupId(groupId), Times.Once);
        _mockStationRepository.Verify(x => x.Update(It.IsAny<Domain.Entities.ChargeStation>()), Times.Once);
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
        var handler = new RemoveChargeStationCommandHandler(_unitOfWork.Object);

        _mockStationRepository.Setup(x => x.GetById(stationId)).ReturnsAsync(chargeStation);

        // Act
        await handler.Handle(removeChargeStationCommand, CancellationToken.None);

        // Assert
        _mockStationRepository.Verify(x => x.GetById(stationId), Times.Once);
        _mockStationRepository.Verify(x => x.Remove(chargeStation), Times.Once);
        _unitOfWork.Verify(x => x.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateChargeStation_ShouldFail_WhenNoGroupIsAssociated()
    {
        var groupId = Guid.NewGuid();
        var chargeStationName = "Test ChargeStation";
        _mockGroupRepository.Setup(x => x.GetById(groupId));

        var createChargeStationCommand = new CreateChargeStationCommand(groupId, chargeStationName);
        var handler = new CreateChargeStationCommandHandler(_unitOfWork.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(createChargeStationCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("group (Parameter 'Group cannot be null')");
    }
}