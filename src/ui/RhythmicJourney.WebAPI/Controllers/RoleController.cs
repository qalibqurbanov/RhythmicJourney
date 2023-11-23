using MediatR;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
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

    /// <summary>
    /// Rollari siyahilamaq ucundur.
    /// </summary>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpGet("get-roles")]
    [SwaggerOperation(Summary = "Rollari siyahilamaq ucundur.", Description = "Bu endpoint vasitesile movcud rollar elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        GetRolesQuery query = new GetRolesQuery();
        RoleResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Roldaki userleri siyahilamaq ucundur.
    /// </summary>
    /// <param name="roleIdentityDTO">Userleri siralanacaq rol.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("get-users-in-role/{RoleID}")]
    [SwaggerOperation(Summary = "Roldaki userleri siyahilamaq ucundur.", Description = "Bu endpoint vasitesile roldaki userler elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetUsersInRole([FromRoute] RoleIdentityDTO roleIdentityDTO, CancellationToken cancellationToken)
    {
        GetUsersInRoleQuery query = new GetUsersInRoleQuery(roleIdentityDTO);
        RoleResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Rol yaratmaq ucundur.
    /// </summary>
    /// <param name="model">Rol yaratmaq ucun daxil edilmiw melumatlar</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("create-role")]
    [SwaggerOperation(Summary = "Rol yaratmaq ucundur.", Description = "Bu endpoint vasitesile yeni bir rol yaradilir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddRole([FromBody] AddRoleRequestDTO model, CancellationToken cancellationToken)
    {
        AddRoleCommand command = new AddRoleCommand(model);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// "Rolun melumatlari uzerinde deyiwiklik etmek ucundur.
    /// </summary>
    /// <param name="model">Uzerinde duzeliw olunan hazirki rolun yeni melumatlari.</param>
    /// <param name="roleIdentityDTO">Melumatlari deyiwdirilecek rol.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("edit-role/{RoleID}")]
    [SwaggerOperation(Summary = "Rolun melumatlari uzerinde deyiwiklik etmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir rol uzerinde deyiwiklik olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> EditRole([FromBody] EditRoleRequestDTO model, [FromRoute] RoleIdentityDTO roleIdentityDTO, CancellationToken cancellationToken)
    {
        EditRoleCommand command = new EditRoleCommand(model, roleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Rolu silmek ucundur.
    /// </summary>
    /// <param name="roleIdentityDTO">Silinecek rol.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpDelete("delete-role/{RoleID}")]
    [SwaggerOperation(Summary = "Rolu silmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir rol silinir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteRole([FromRoute] RoleIdentityDTO roleIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteRoleCommand command = new DeleteRoleCommand(roleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Rola user elave etmek ucundur.
    /// </summary>
    /// <param name="userAndRoleIdentityDTO">Hansi rola hansi user elave olunacaq.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("add-user-to-role")]
    [SwaggerOperation(Summary = "Rola user elave etmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir rola her hansi bir user elave olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddUserToRole([FromBody] UserAndRoleIdentityDTO userAndRoleIdentityDTO, CancellationToken cancellationToken)
    {
        AddUserToRoleCommand command = new AddUserToRoleCommand(userAndRoleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Roldan useri xaric etmek ucundur.
    /// </summary>
    /// <param name="userAndRoleIdentityDTO">Hansi roldan hansi user xaric olunacaq.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpDelete("delete-user-from-role")]
    [SwaggerOperation(Summary = "Roldan useri xaric etmek ucundur.", Description = "Bu endpoint vasitesile mueyyen bir roldaki user hemin roldan xaric edilir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteUserFromRole([FromBody] UserAndRoleIdentityDTO userAndRoleIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteUserFromRoleCommand command = new DeleteUserFromRoleCommand(userAndRoleIdentityDTO);
        RoleResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
}