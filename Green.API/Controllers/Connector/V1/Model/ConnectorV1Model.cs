namespace Green.API.Controllers.Connector.V1.Model;

public class ConnectorV1Model
{
    public Guid StationId { get; set; }
    public int Identifier { get; set; }
    public int MaxCurrentInAmps { get; set; }
}
