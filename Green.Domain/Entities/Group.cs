using Green.Domain.DomainEvents;
using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class Group : Entity
{
    private readonly List<ChargeStation> _chargeStations;

    public string Name { get; private set; }
    public int CapacityInAmps { get; private set; }

    public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations.AsReadOnly();

    protected Group() { _chargeStations = new(); }
    public Group(string name, int capacityInAmps)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be empty");

        if (capacityInAmps <= 0)
            throw new InvalidOperationException("Capacity must be greater than zero");

        Name = name;
        CapacityInAmps = capacityInAmps;
        _chargeStations = new();
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be empty");

        Name = name;
    }

    public void ChangeCapacity(int capacity)
    {
        if (capacity <= 0)
            throw new InvalidOperationException("Capacity must be greater than zero");

        CapacityInAmps = capacity;
    }

    public void RemoveChargeStations()
    {
        RaiseDomainEvent(new GroupRemovedDomainEvent(Id));
    }

    public bool HasSufficientGroupCapacity(double totalConnectorCurrent)
    {
        return CapacityInAmps >= totalConnectorCurrent;
    }
}
