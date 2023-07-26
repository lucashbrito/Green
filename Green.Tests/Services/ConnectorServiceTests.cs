using FluentAssertions;
using Green.Application.Services;
using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Moq;

namespace Green.Tests.Services;

public class ConnectorServiceTests
{
    private Mock<IChargeStationRepository> _mockStationRepository;
    private Mock<IConnectorRepository> _mockConnectorRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    private ConnectorService _service;

    public ConnectorServiceTests()
    {
        _mockStationRepository = new Mock<IChargeStationRepository>();
        _mockConnectorRepository = new Mock<IConnectorRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _service = new ConnectorService(_mockStationRepository.Object, _mockConnectorRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateConnector_ShouldCreateConnector_WhenCalledWithValidData()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var station = new ChargeStation("Test Station", new Group("Test Group", 100));

        _mockStationRepository.Setup(m => m.GetById(stationId)).ReturnsAsync(station);

        // Act
        var result = await _service.CreateConnector(stationId, 1, 20);

        // Assert
        result.Should().NotBeNull();
        result.Identifier.Should().Be(1);
        result.MaxCurrentInAmps.Should().Be(20);
        result.ChargeStationId.Should().Be(station.Id);
        _mockConnectorRepository.Verify(m => m.Add(It.IsAny<Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateConnectorMaxCurrent_ShouldUpdateConnectorMaxCurrent_WhenCalledWithValidData()
    {
        // Arrange
        var connectorId = Guid.NewGuid();
        var connector = new Connector(1, 20, new ChargeStation("Test Station", new Group("Test Group", 100)));
        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        // Act
        await _service.UpdateConnectorMaxCurrent(connectorId, 30);

        // Assert
        connector.MaxCurrentInAmps.Should().Be(30);
        _mockConnectorRepository.Verify(m => m.Update(It.IsAny<Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveConnector_ShouldRemoveConnector_WhenCalledWithValidData()
    {
        // Arrange
        var connectorId = Guid.NewGuid();
        var connector = new Connector(1, 20, new ChargeStation("Test Station", new Group("Test Group", 100)));
        _mockConnectorRepository.Setup(m => m.GetById(connectorId)).ReturnsAsync(connector);

        // Act
        await _service.RemoveConnector(Guid.NewGuid(), connectorId);

        // Assert
        _mockConnectorRepository.Verify(m => m.Remove(It.IsAny<Connector>()), Times.Once);
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveConnectorByChargeStation_ShouldRemoveConnector_WhenCalledWithValidData()
    {
        // Arrange
        var stationId = Guid.NewGuid();
        var connectors = new List<Connector>
        {
            new Connector(1, 20, new ChargeStation("Test Station", new Group("Test Group", 100))),
            new Connector(2, 30, new ChargeStation("Test Station", new Group("Test Group", 100)))
        };

        _mockConnectorRepository.Setup(m => m.GetByChargeStationId(stationId)).ReturnsAsync(connectors);

        // Act
        await _service.RemoveConnectorByChargeStation(stationId);

        // Assert
        _mockConnectorRepository.Verify(m => m.Remove(It.IsAny<Connector>()), Times.Exactly(connectors.Count));
        _mockUnitOfWork.Verify(m => m.CompleteAsync(), Times.Once);
    }
}
