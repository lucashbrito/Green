using Green.Domain.Abstractions.IRepositories;

namespace Green.Domain.Abstractions;

public interface IUnitOfWork
{
    public IChargeStationRepository ChargeStationRepository { get; }
    public IConnectorRepository ConnectorRepository { get;  }
    public IGroupRepository GroupRepository { get; }

    Task CompleteAsync(CancellationToken cancellationToken);
}
