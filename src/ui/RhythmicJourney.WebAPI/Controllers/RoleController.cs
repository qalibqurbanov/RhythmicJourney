using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Queries;
using RhythmicJourney.Application.Features.Role.Commands;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "MODERATOR, ADMIN")]
public class RoleController : Controller
{
    private readonly IMediator _mediator;
    public RoleController(IMediator mediator) => this._mediator = mediator;

    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        GetRolesQuery query = new GetRolesQuery();
        RoleResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("get-users-in-role/{RoleID}")]
    public async Task<IActionResult> GetUsersInRole([FromRoute] RoleIdentityDTO model, CancellationToken cancellationToken)
    {
        GetUsersInRoleQuery query = new GetUsersInRoleQuery(model);
        RoleResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("create-role")]
    public async Task<IActionResult> AddRole([FromBody] AddRoleRequestDTO model, CancellationToken cancellationToken)
    {
        AddRoleCommand command = new AddRoleCommand(model);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("edit-role/{RoleID}")]
    public async Task<IActionResult> EditRole([FromBody] EditRoleRequestDTO model, [FromRoute] RoleIdentityDTO roleIdentityDTO, CancellationToken cancellationToken)
    {
        EditRoleCommand command = new EditRoleCommand(model, roleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("delete-role/{RoleID}")]
    public async Task<IActionResult> DeleteRole([FromRoute] RoleIdentityDTO roleIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteRoleCommand command = new DeleteRoleCommand(roleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("add-user-to-role")]
    public async Task<IActionResult> AddUserToRole([FromBody] UserAndRoleIdentityDTO userAndRoleIdentityDTO, CancellationToken cancellationToken)
    {
        AddUserToRoleCommand command = new AddUserToRoleCommand(userAndRoleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("delete-user-from-role")]
    public async Task<IActionResult> DeleteUserFromRole([FromBody] UserAndRoleIdentityDTO userAndRoleIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteUserFromRoleCommand command = new DeleteUserFromRoleCommand(userAndRoleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
}