using Green.API.Controllers.Group.V1.Model;
using Green.Domain.Abstractions.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.Group.V1;

[ApiController]
[Route("api/group")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _service;

    public GroupController(IGroupService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] GroupV1Model groupModel)
    {
        var group = await _service.CreateGroup(groupModel.Name, groupModel.CapacityInAmps);
        return Created($"/api/group/{group.Id}", group);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] GroupV1Model groupModel)
    {
        await _service.UpdateGroup(id, groupModel.Name, groupModel.CapacityInAmps);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveGroup(Guid id)
    {
        await _service.RemoveGroup(id);
        return NoContent();
    }
}
