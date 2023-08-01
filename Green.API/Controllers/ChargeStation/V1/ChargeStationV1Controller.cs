using Green.API.Controllers.ChargeStation.V1.Model;
using Green.Application.ChargeStation.Commands;
using Green.Application.ChargeStation.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.ChargeStation.V1
{
    [ApiController]
    [Route("api/chargeStation")]
    public class ChargeStationV1Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChargeStationV1Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateChargeStation(ChargeStationV1Model chargeStationModel)
        {
            var chargeStation = await _mediator.Send(new CreateChargeStationCommand(chargeStationModel.GroupId, chargeStationModel.Name));
            return chargeStation.IsSuccess ? Created($"/api/chargeStation/{chargeStation.Value.Id}", chargeStation) : BadRequest();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllChargeStationsQuery()));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChargeStation(Guid id, ChargeStationV1Model chargeStationModel)
        {
            await _mediator.Send(new UpdateChargeStationCommand(id, chargeStationModel.Name, chargeStationModel.GroupId));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveChargeStation(Guid id)
        {
            await _mediator.Send(new RemoveChargeStationCommand(id));
            return NoContent();
        }
    }
}