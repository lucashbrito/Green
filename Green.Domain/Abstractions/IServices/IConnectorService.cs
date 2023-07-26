﻿using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IServices;

public interface IConnectorService
{
    Task<Connector> CreateConnector(Guid stationId, int identifier, int maxCurrentInAmps);
    Task UpdateConnectorMaxCurrent(Guid connectorId, int maxCurrentInAmps);
    Task RemoveConnector(Guid stationId, Guid connectorId);
    Task RemoveConnectorByChargeStation(Guid stationId);
}
