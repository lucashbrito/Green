using Green.Domain.Entities;
using Green.Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Green.Infrastructure;

public class GreenDbContext : DbContext
{
    private IPublisher _publisher;
    public DbSet<Group> Groups { get; set; }
    public DbSet<ChargeStation> ChargeStations { get; set; }
    public DbSet<Connector> Connectors { get; set; }

    public GreenDbContext(DbContextOptions<GreenDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
          .HasMany(g => g.ChargeStations)
          .WithOne();
     
        modelBuilder.Entity<ChargeStation>()
            .HasMany(c => c.Connectors)
            .WithOne();
    
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entities =>
            {
                var domainEvents = entities.GetDomainEvents();

                entities.ClearDomainEvents();

                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
