using Green.Domain.DomainEvents;
using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class ChargeStation : Entity
{
    private List<Connector> _connectors;
    public string Name { get; private set; }
    public Guid GroupId { get; private set; }

    public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();

    protected ChargeStation() { }

    public ChargeStation(string name, Group group)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        SetGroup(group);

        _connectors = new();
    }

    private static void IsGroupNull(Group group)
    {
        if (group is null)
            throw new ArgumentNullException("Group cannot be null", nameof(group));
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
        Name = name;
    }

    public void SetGroup(Group group)
    {
        IsGroupNull(group);

        if (GroupId == group.Id)
            return;

        GroupId = group.Id;
    }

    public void RemoveConnectors()
    {
        RaiseDomainEvent(new ChargeStationRemovedDomainEvent(Id));
    }
}
