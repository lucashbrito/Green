using Green.API.Controllers.ChargeStation.V1.Model;
using Green.Domain.Abstractions.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.ChargeStation.V1
{
    [ApiController]
    [Route("api/chargeStation")]
    public class ChargeStationV1Controller : ControllerBase
    {
        private readonly IChargeStationService _service;

        public ChargeStationV1Controller(IChargeStationService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateChargeStation(ChargeStationV1Model chargeStationModel)
        {
            var chargeStation = await _service.CreateChargeStation(chargeStationModel.GroupId, chargeStationModel.Name);
            return Created($"/api/chargeStation/{chargeStation.Id}", chargeStation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChargeStation(Guid id, ChargeStationV1Model chargeStationModel)
        {
            await _service.UpdateChargeStation(id, chargeStationModel.Name, chargeStationModel.GroupId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveChargeStation(Guid id)
        {
            await _service.RemoveChargeStation(id);
            return NoContent();
        }
    }
}