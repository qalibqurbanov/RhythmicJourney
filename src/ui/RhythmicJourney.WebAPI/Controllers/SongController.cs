using MediatR;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Queries;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "USER")]
public class SongController : ControllerBase
{
    private readonly IMediator _mediator;
    public SongController(IMediator mediator) => this._mediator = mediator;

    /// <summary>
    /// Musiqileri siyahilamaq ucundur.
    /// </summary>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpGet("get-songs")]
    [SwaggerOperation(Summary = "Musiqileri siyahilamaq ucundur.", Description = "Bu endpoint vasitesile hazirki userin elave etdiyi musiqiler elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetSongs(CancellationToken cancellationToken)
    {
        GetSongsQuery query = new GetSongsQuery();
        SongResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Musiqi upload etmek ucundur.
    /// </summary>
    /// <param name="model">Upload edilmek istenen musiqinin melumatlari</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("add-song")]
    [SwaggerOperation(Summary = "Musiqi upload etmek ucundur.", Description = "Bu endpoint vasitesile hazirki user sistemimize musiqi upload edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddSong([FromForm] AddSongRequestDTO model, CancellationToken cancellationToken)
    /*
        * Web API def olaraq requestin bodysini/payloadini 'application/json' olaraq post edir. Lakin men userden 'IFormFile' yeni, fayl alacam deye gerek '[FromForm]' atributuyla datalari formdan alacagimi bildirim ve belece datalar avtomatik bir wekilde "multipart/form-data" olaraq gonderilecek bize(servere).
    */
    {
        AddSongCommand command = new AddSongCommand(model);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Musiqinin melumatlari uzerinde deyiwiklik etmek ucundur.
    /// </summary>
    /// <param name="model">Uzerinde duzeliw olunan hazirki musiqinin yeni melumatlari.</param>
    /// <param name="songIdentityDTO">Melumatlari deyiwdirilecek musiqi.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("edit-song/{SongID}")] /* int SongID = Convert.ToInt32(HttpContext.GetRouteValue(nameof(SongID))); */
    [SwaggerOperation(Summary = "Musiqinin melumatlari uzerinde deyiwiklik etmek ucundur.", Description = "Bu endpoint vasitesile hazirki userin elave etdiyi musiqiler elde olunur.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> EditSong([FromForm] EditSongRequestDTO model, [FromRoute] SongIdentityDTO songIdentityDTO, CancellationToken cancellationToken)
    {
        EditSongCommand command = new EditSongCommand(model, songIdentityDTO);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Musiqini silmek ucundur.
    /// </summary>
    /// <param name="songIdentityDTO">Silinecek musiqi.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpDelete("remove-song/{SongID}")]
    [SwaggerOperation(Summary = "Musiqini silmek ucundur.", Description = "Bu endpoint vasitesile hazirki user sistemimize ne vaxtsa upload etmiw oldugu musiqisini silir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RemoveSong([FromRoute] SongIdentityDTO songIdentityDTO, CancellationToken cancellationToken)
    {
        DeleteSongCommand command = new DeleteSongCommand(songIdentityDTO);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
}