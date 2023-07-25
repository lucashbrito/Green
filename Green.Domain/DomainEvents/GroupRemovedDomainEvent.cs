using Green.Domain.Primitives;

namespace Green.Domain.DomainEvents;

public class GroupRemovedDomainEvent : IDomainEvent
{
    public Guid GroupId { get; protected set; }

    public GroupRemovedDomainEvent(Guid groupId)
    {
        GroupId = groupId;
    }
}
