using Green.Domain.Abstractions;
using Green.Domain.Primitives;
using MediatR;

namespace Green.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPublisher _publisher;
        private readonly GreenDbContext _context;

        public UnitOfWork(GreenDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
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
}
