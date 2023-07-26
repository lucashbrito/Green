namespace Green.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents?.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public Guid Id { get; private init; }

    protected Entity()
    {
        Id = Guid.NewGuid();

    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == default || other.Id == default)
            return false;

        return Id == other.Id;
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        return a is not null && b is not null && a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}