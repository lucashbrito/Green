using Green.API.Controllers.Connector.V1.Model;
using Green.Domain.Abstractions.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.Connector.V1
{
    [ApiController]
    [Route("api/connector")]
    public class ConnectorController : ControllerBase
    {
        private readonly IConnectorService _service;

        public ConnectorController(IConnectorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConnector([FromBody] ConnectorV1Model connectorModel)
        {
            var connector = await _service.CreateConnector(connectorModel.StationId, connectorModel.Identifier, connectorModel.MaxCurrentInAmps);
            return Created($"/api/connector/{connector.Id}", connector);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConnectorMaxCurrent(Guid id, [FromBody] ConnectorV1Model connectorModel)
        {
            await _service.UpdateConnectorMaxCurrent(id, connectorModel.MaxCurrentInAmps);
            return NoContent();
        }

        [HttpDelete("{connectorId}")]
        public async Task<IActionResult> RemoveConnector(Guid connectorId)
        {
            await _service.RemoveConnector(connectorId);
            return NoContent();
        }
    }
}
