using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;

namespace Green.Application.Connector;

public class ConnectorService : IConnectorService
{
    private IConnectorRepository _connectorRepository;

    public ConnectorService(IConnectorRepository connectorRepository)
    {
        _connectorRepository = connectorRepository;
    }

    public async Task RemoveConnectorByChargeStation(Guid stationId)
    {
        var connectors = await _connectorRepository.GetByChargeStationId(stationId);

        if (connectors is null || connectors.Count == 0)
            return;

        foreach (var connector in connectors)
            _connectorRepository.Remove(connector);
    }
}
