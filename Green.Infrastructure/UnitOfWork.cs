using Green.Domain.Abstractions;
using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Primitives;
using MediatR;

namespace Green.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IPublisher _publisher;
    private readonly GreenDbContext _context;

    private readonly IChargeStationRepository _chargeStationRepository;
    private readonly IConnectorRepository _connectorRepository;
    private readonly IGroupRepository _groupRepository;

    public IChargeStationRepository ChargeStationRepository => _chargeStationRepository;
    public IConnectorRepository ConnectorRepository => _connectorRepository;
    public IGroupRepository GroupRepository => _groupRepository;


    public UnitOfWork(GreenDbContext context,
        IPublisher publisher,
        IChargeStationRepository chargeStationRepository,
        IConnectorRepository connectorRepository,
        IGroupRepository groupRepository)
    {
        _context = context;
        _publisher = publisher;
        _chargeStationRepository = chargeStationRepository;
        _connectorRepository = connectorRepository;
        _groupRepository = groupRepository;
    }


    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        await PublishDomainEventsAsync(cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    protected async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = GetDomainEventsFromTrackedEntities();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    private List<IDomainEvent> GetDomainEventsFromTrackedEntities()
    {
        var domainEvents = _context.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                var events = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return events;
            })
            .ToList();

        return domainEvents;
    }
}
