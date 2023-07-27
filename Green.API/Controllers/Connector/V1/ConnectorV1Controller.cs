using Green.API.Controllers.Connector.V1.Model;
using Green.Application.Connector.Commands;
using Green.Application.Connector.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.Connector.V1
{
    [ApiController]
    [Route("api/connector")]
    public class ConnectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConnectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConnector([FromBody] ConnectorV1Model connectorModel)
        {
            var connector = await _mediator.Send(new CreateConnectorCommand(connectorModel.StationId, connectorModel.Identifier, connectorModel.MaxCurrentInAmps));
            return Created($"/api/connector/{connector.Id}", connector);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllConnectorsQuery()));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConnectorMaxCurrent(Guid id, [FromBody] ConnectorV1Model connectorModel)
        {
            await _mediator.Send(new UpdateConnectorMaxCurrentCommand(id, connectorModel.MaxCurrentInAmps));
            return NoContent();
        }

        [HttpDelete("{connectorId}")]
        public async Task<IActionResult> RemoveConnector(Guid connectorId)
        {
            await _mediator.Send(new RemoveConnectorCommand(connectorId));
            return NoContent();
        }
    }
}
