using Green.API.Controllers.Group.V1.Model;
using Green.Application.Group.Commands;
using Green.Application.Group.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Green.API.Controllers.Group.V1;

[ApiController]
[Route("api/group")]
public class GroupV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] GroupV1Model groupModel, CancellationToken cancellationToken)
    {
        var group = await _mediator.Send(new CreateGroupCommand(groupModel.Name, groupModel.CapacityInAmps));

        return Created($"/api/group/{group.Id}", group);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllGroupsQuery()));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] GroupV1Model groupModel, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateGroupCommand(id, groupModel.Name, groupModel.CapacityInAmps));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveGroup(Guid id)
    {
        await _mediator.Send(new RemoveGroupCommand(id));
        return NoContent();
    }
}
