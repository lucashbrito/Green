using Green.Domain.Primitives;

namespace Green.Domain.Entities;

public class Connector : Entity
{
    public int Identifier { get; private set; }
    public int MaxCurrentInAmps { get; private set; }
    public Guid ChargeStationId { get; private set; }

    protected Connector() { }
    public Connector(int identifier, int maxCurrentInAmps, ChargeStation chargeStation)
    {
        IsBetweenOneAndFive(identifier);

        IsMaxCurrentGreaterThanZero(maxCurrentInAmps);

        Identifier = identifier;
        MaxCurrentInAmps = maxCurrentInAmps;

        if (chargeStation is null)
            throw new ArgumentNullException("Charge station not found",nameof(chargeStation));

        ChargeStationId = chargeStation.Id;
    }

    private static void IsMaxCurrentGreaterThanZero(int maxCurrentInAmps)
    {
        if (maxCurrentInAmps <= 0)
            throw new ArgumentException("Max current must be greater than zero", nameof(maxCurrentInAmps));
    }

    private static void IsBetweenOneAndFive(int identifier)
    {
        if (identifier < 1 || identifier > 5)
            throw new ArgumentException("Identifier must be between 1 and 5", nameof(identifier));
    }

    public void ChangeMaxCurrent(int maxCurrent)
    {
        IsMaxCurrentGreaterThanZero(maxCurrent);
        MaxCurrentInAmps = maxCurrent;
    }
}
