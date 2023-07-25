using Green.Domain.Abstractions;

namespace Green.Application.Connector;

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

    public async Task<Domain.Connector> CreateConnector(Guid stationId, int identifier, int maxCurrentInAmps)
    {
        var station = await _chargeStationRepository.GetById(stationId);

        var connector = new Domain.Connector(identifier, maxCurrentInAmps, station);

        // station.AddConnector(connector);

        _connectorRepository.Add(connector);

        //_chargeStationRepository.Add(station); vai ter que ter um n p n? 

        await _unitOfWork.CompleteAsync();

        return connector;
    }

    public async Task UpdateConnectorMaxCurrent(Guid connectorId, int maxCurrentInAmps)
    {
        Domain.Connector connector = await _connectorRepository.GetById(connectorId);
        connector.ChangeMaxCurrent(maxCurrentInAmps);

        _connectorRepository.Update(connector);
        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveConnector(Guid stationId, Guid connectorId)
    {
        var station = await _chargeStationRepository.GetById(stationId);
        var connector = await _connectorRepository.GetById(connectorId);

        if (connector is null)
            return;

        //o certo aqui seria criar um event handler para o evento de remocao de station
        //station.RemoveConnector(connector);

        _connectorRepository.Remove(connector);

        await _unitOfWork.CompleteAsync();
    }
}
