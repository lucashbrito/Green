using Green.Domain.Primitives;

namespace Green.Domain.DomainEvents;
public class ChargeStationRemovedDomainEvent : IDomainEvent
{
    public Guid ChargeStationId { get; protected set; }

    public ChargeStationRemovedDomainEvent(Guid chargeStationId)
    {
        ChargeStationId = chargeStationId;
    }
}
