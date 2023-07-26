using Green.Domain.DomainEvents;
using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class Group : Entity
{
    private List<ChargeStation> _chargeStations;

    public string Name { get; private set; }
    public int CapacityInAmps { get; private set; }

    public IReadOnlyCollection<ChargeStation> ChargeStations => _chargeStations.AsReadOnly();

    protected Group() { }
    public Group(string name, int capacityInAmps)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (capacityInAmps <= 0)
            throw new ArgumentException("Capacity must be greater than zero", nameof(capacityInAmps));

        Name = name;
        CapacityInAmps = capacityInAmps;
        _chargeStations = new();
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    public void ChangeCapacity(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero", nameof(capacity));

        CapacityInAmps = capacity;
    }

    public void AddChargeStation(ChargeStation chargeStation)
    {
        if (_chargeStations.Count >= 5)
            throw new InvalidOperationException("Cannot add more than 5 charge stations to a group");

        _chargeStations.Add(chargeStation);
    }

    public void RemoveChargeStations()
    {
        RaiseDomainEvent(new GroupRemovedDomainEvent(Id));
    }
}
