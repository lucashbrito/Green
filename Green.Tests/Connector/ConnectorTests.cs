using FluentAssertions;
using Green.Application.Connector.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Moq;

namespace Green.Tests.Connector;
public class ConnectorCommandHandlerTests
{
    private Mock<IChargeStationRepository> _mockStationRepository;
    private Mock<IConnectorRepository> _mockConnectorRepository;
    private Mock<IGroupRepository> _mockGroupRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IGroupServices> _mockGroupService;

    public ConnectorCommandHandlerTests()
    {
        _mockStationRepository = new Mock<IChargeStationRepository>();
        _mockConnectorRepository = new Mock<IConnectorRepository>();
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockGroupService = new Mock<IGroupServices>();

        _mockUnitOfWork.Setup(u => u.ChargeStationRepository).Returns(_mockStationRepository.Object);
        _mockUnitOfWork.Setup(u => u.GroupRepository).Returns(_mockGroupRepository.Object);
        _mockUnitOfWork.Setup(u => u.ConnectorRepository).Returns(_mockConnectorRepository.Object);
    }

    [Fact]
    public async Task UpdateConnectorMaxCurrent_ShouldUpdateConnectorMaxCurrent_WhenCalledWithValidData()
    {
        // Arrange
        var connectorId = Guid.NewGuid();
        var station = new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100));
        var connector = new Domain.Entities.Connector(1, 20, station.Id);

        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        var command = new UpdateConnectorMaxCurrentCommand(connectorId, 30);
        var handler = new UpdateConnectorMaxCurrentCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        connector.MaxCurrentInAmps.Should().Be(30);
        _mockConnectorRepository.Verify(m => m.Update(It.IsAny<Domain.Entities.Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveConnector_ShouldRemoveConnector_WhenCalledWithValidData()
    {
        // Arrange
        var connectorId = Guid.NewGuid();
        var station = new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100));
        var connector = new Domain.Entities.Connector(1, 20, station.Id);

        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        var command = new RemoveConnectorCommand(connectorId);
        var handler = new RemoveConnectorCommandHandler(_mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockConnectorRepository.Verify(m => m.Remove(It.IsAny<Domain.Entities.Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddConnectorCommandHandler_ShouldSucceed_WhenMaxCurrentSumDoesNotExceedGroupCapacity()
    {
        // Arrange          
        var group = new Domain.Entities.Group("Test Group", 100);
        var station = new Domain.Entities.ChargeStation("Test Station", group);
        var connectors = new List<Domain.Entities.Connector>
    {
        new Domain.Entities.Connector(1, 20, station.Id),
        new Domain.Entities.Connector(2, 30, station.Id)
    };
        _mockGroupRepository.Setup(m => m.GetById(group.Id)).ReturnsAsync(group);
        _mockStationRepository.Setup(m => m.GetById(station.Id)).ReturnsAsync(station);
        _mockStationRepository.Setup(m => m.GetByGroupId(group.Id)).ReturnsAsync(new List<Domain.Entities.ChargeStation>() { station });
        _mockConnectorRepository.Setup(m => m.GetByChargeStationId(station.Id)).ReturnsAsync(connectors);

        var command = new CreateConnectorCommand(station.Id, 3, 40);
        var handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Identifier.Should().Be(3);
        result.MaxCurrentInAmps.Should().Be(40);
        _mockConnectorRepository.Verify(m => m.Add(It.IsAny<Domain.Entities.Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddConnectorCommandHandler_ShouldFail_WhenMaxCurrentSumExceedsGroupCapacity()
    {
        // Arrange
        var group = new Domain.Entities.Group("Test Group", 100);
        var station = new Domain.Entities.ChargeStation("Test Station", group);
        var connectors = new List<Domain.Entities.Connector>
    {
        new Domain.Entities.Connector(1, 50, station.Id),
        new Domain.Entities.Connector(2, 60, station.Id)
    };

        _mockGroupRepository.Setup(m => m.GetById(group.Id)).ReturnsAsync(group);
        _mockStationRepository.Setup(m => m.GetById(station.Id)).ReturnsAsync(station);
        _mockStationRepository.Setup(m => m.GetByGroupId(group.Id)).ReturnsAsync(new List<Domain.Entities.ChargeStation>() { station });
        _mockConnectorRepository.Setup(m => m.GetByChargeStationId(station.Id)).ReturnsAsync(connectors);

        var command = new CreateConnectorCommand(station.Id, 3, 50);
        var handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task CreateConnector_ShouldFail_WhenChargeStationIsNull()
    {
        _mockStationRepository.Setup(m => m.GetById(It.IsAny<Guid>()));

        // Arrange
        var command = new CreateConnectorCommand(Guid.NewGuid(), 1, 20);
        var handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("station (Parameter 'station not found')");
    }

    [Fact]
    public async Task CreateConnector_ShouldFail_WhenIdentifierIsNotBetweenOneAndFive()
    {
        // Arrange
        var station = new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100));

        _mockStationRepository.Setup(m => m.GetById(It.IsAny<Guid>())).ReturnsAsync(station);

        var command = new CreateConnectorCommand(station.Id, 6, 20);
        var handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Identifier must be between 1 and 5 (Parameter 'Identifier')");
    }

    [Fact]
    public async Task CreateConnector_ShouldFail_WhenMaxCurrentIsNotGreaterThanZero()
    {
        // Arrange
        var station = new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100));

        _mockStationRepository.Setup(m => m.GetById(It.IsAny<Guid>())).ReturnsAsync(station);

        var command = new CreateConnectorCommand(Guid.NewGuid(), 1, 0);
        var handler = new CreateConnectorCommandHandler(_mockUnitOfWork.Object, _mockGroupService.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Max current must be greater than zero (Parameter 'maxCurrentInAmps')");
    }
}
