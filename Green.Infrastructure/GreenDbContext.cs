using Green.Domain.Entities;
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
}
