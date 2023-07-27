using FluentAssertions;
using Green.Application.Connector.Commands;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Moq;

namespace Green.Tests.Connector;
public class ConnectorCommandHandlerTests
{
    private Mock<IChargeStationRepository> _mockStationRepository;
    private Mock<IConnectorRepository> _mockConnectorRepository;
    private Mock<IGroupRepository> _mockGroupRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    public ConnectorCommandHandlerTests()
    {
        _mockStationRepository = new Mock<IChargeStationRepository>();
        _mockConnectorRepository = new Mock<IConnectorRepository>();
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task CreateConnector_ShouldCreateConnector_WhenCalledWithValidData()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var station = new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100));

        _mockStationRepository.Setup(m => m.GetById(stationId)).ReturnsAsync(station);

        var command = new CreateConnectorCommand(stationId, 1, 20);
        var handler = new CreateConnectorCommandHandler(_mockStationRepository.Object, _mockConnectorRepository.Object, _mockUnitOfWork.Object, _mockGroupRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Identifier.Should().Be(1);
        result.MaxCurrentInAmps.Should().Be(20);
        result.ChargeStationId.Should().Be(station.Id);
        _mockConnectorRepository.Verify(m => m.Add(It.IsAny<Domain.Entities.Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateConnectorMaxCurrent_ShouldUpdateConnectorMaxCurrent_WhenCalledWithValidData()
    {
        // Arrange
        var connectorId = Guid.NewGuid();
        var connector = new Domain.Entities.Connector(1, 20, new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100)));

        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        var command = new UpdateConnectorMaxCurrentCommand(connectorId, 30);
        var handler = new UpdateConnectorMaxCurrentCommandHandler(_mockConnectorRepository.Object, _mockUnitOfWork.Object);

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
        var connector = new Domain.Entities.Connector(1, 20, new Domain.Entities.ChargeStation("Test Station", new Domain.Entities.Group("Test Group", 100)));

        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        var command = new RemoveConnectorCommand(connectorId);
        var handler = new RemoveConnectorCommandHandler(_mockConnectorRepository.Object, _mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockConnectorRepository.Verify(m => m.Remove(It.IsAny<Domain.Entities.Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
