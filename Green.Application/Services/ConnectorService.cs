using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Abstractions.IServices;
using Green.Domain.Entities;

namespace Green.Application.Services;

public class ConnectorService : IConnectorService
{
    private IChargeStationRepository _chargeStationRepository;
    private IConnectorRepository _connectorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConnectorService(IChargeStationRepository stationRepository, IConnectorRepository connectorRepository, IUnitOfWork unitOfWork)
    {
        _chargeStationRepository = stationRepository;
        _connectorRepository = connectorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Connector> CreateConnector(Guid stationId, int identifier, int maxCurrentInAmps)
    {
        var station = await _chargeStationRepository.GetById(stationId);

        var connector = new Connector(identifier, maxCurrentInAmps, station);

        _connectorRepository.Add(connector);

        await _unitOfWork.CompleteAsync();

        return connector;
    }

    public async Task UpdateConnectorMaxCurrent(Guid connectorId, int maxCurrentInAmps)
    {
        Connector connector = await _connectorRepository.GetById(connectorId);
        connector.ChangeMaxCurrent(maxCurrentInAmps);

        _connectorRepository.Update(connector);
        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveConnector( Guid connectorId)
    {
        var connector = await _connectorRepository.GetById(connectorId);

        if (connector is null)
            return;

        _connectorRepository.Remove(connector);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveConnectorByChargeStation(Guid stationId)
    {
        var connectors = await _connectorRepository.GetByChargeStationId(stationId);

        if (connectors is null)
            return;

        foreach (var connector in connectors)
            _connectorRepository.Remove(connector);

        await _unitOfWork.CompleteAsync();
    }
}
