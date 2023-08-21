using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IServices;

namespace Green.Application.Connector;

public class ConnectorService : IConnectorService
{
    private readonly IUnitOfWork _unitOfWork;

    public ConnectorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveConnectorByChargeStation(Guid stationId)
    {
        var connectors = await _unitOfWork.ConnectorRepository.GetByChargeStationId(stationId);

        if (connectors is null || connectors.Count == 0)
            return;

        foreach (var connector in connectors)
            _unitOfWork.ConnectorRepository.Remove(connector);
    }
}
