using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class Connector : Entity
{
    public int Identifier { get; private set; }
    public int MaxCurrentInAmps { get; private set; }
    public Guid ChargeStationId { get; private set; }

    protected Connector() { }
    public Connector(int identifier, int maxCurrentInAmps, Guid chargeStationId)
    {
        IsBetweenOneAndFive(identifier);

        IsMaxCurrentGreaterThanZero(maxCurrentInAmps);

        Identifier = identifier;
        MaxCurrentInAmps = maxCurrentInAmps;
        ChargeStationId = chargeStationId;
    }

    private static void IsMaxCurrentGreaterThanZero(int maxCurrentInAmps)
    {
        if (maxCurrentInAmps <= 0)
            throw new InvalidOperationException("Max current must be greater than zero");
    }

    private static void IsBetweenOneAndFive(int identifier)
    {
        if (identifier < 1 || identifier > 5)
            throw new InvalidOperationException("Identifier must be between 1 and 5");
    }

    public void ChangeMaxCurrent(int maxCurrent)
    {
        IsMaxCurrentGreaterThanZero(maxCurrent);
        MaxCurrentInAmps = maxCurrent;
    }
}
