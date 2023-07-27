using Green.Domain.Entities;

namespace Green.Domain.Abstractions.IServices;

public interface IConnectorService
{  
    Task RemoveConnectorByChargeStation(Guid stationId);
}
