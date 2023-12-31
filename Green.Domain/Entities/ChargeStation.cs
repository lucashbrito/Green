﻿using Green.Domain.DomainEvents;
using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class ChargeStation : Entity
{
    private readonly List<Connector> _connectors;
    public string Name { get; private set; }
    public Guid GroupId { get; private set; }

    public IReadOnlyCollection<Connector> Connectors => _connectors.AsReadOnly();

    protected ChargeStation() { _connectors = new(); }

    public ChargeStation(string name, Group group)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be empty");

        Name = name;
        SetGroup(group);

        _connectors = new();
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be empty");

        Name = name;
    }

    public void SetGroup(Group group)
    {
        if (GroupId == group.Id)
            return;

        GroupId = group.Id;
    }

    public void RemoveConnectors()
    {
        RaiseDomainEvent(new ChargeStationRemovedDomainEvent(Id));
    }
}
