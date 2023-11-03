using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Queries;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SongController : ControllerBase
{
    private readonly IMediator _mediator;
    public SongController(IMediator mediator) => this._mediator = mediator;

    [HttpGet("get-songs")]
    public async Task<IActionResult> GetSongs(CancellationToken cancellationToken)
    {
        GetSongsQuery query = new GetSongsQuery();
        SongResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost("add-song")]
    public async Task<IActionResult> AddSong([FromForm] AddSongRequestDTO model, CancellationToken cancellationToken)
    /*
        * Web API def olaraq requestin bodysini/payloadini 'application/json' olaraq post edir. Lakin men userden 'IFormFile' yeni, fayl alacam deye gerek '[FromForm]' atributuyla datalari formdan alacagimi bildirim ve belece datalar avtomatik bir wekilde "multipart/form-data" olaraq gonderilecek bize(servere).
    */
    {
        AddSongCommand command = new AddSongCommand(model);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if(!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }

    [HttpPost("edit-song/{SongID}")] /* int SongID = Convert.ToInt32(HttpContext.GetRouteValue(nameof(SongID))); */
    public async Task<IActionResult> EditSong([FromForm] EditSongRequestDTO model, [FromRoute] SongIdentityDTO songIdentityDTO, CancellationToken cancellationToken)
    {
        EditSongCommand command = new EditSongCommand(model, songIdentityDTO);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }

    [HttpDelete("remove-song/{SongID}")]
    public async Task<IActionResult> RemoveSong([FromRoute] SongIdentityDTO model, CancellationToken cancellationToken)
    {
        DeleteSongCommand command = new DeleteSongCommand(model);
        SongResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status400BadRequest,
            title: string.Join(Environment.NewLine, result.Errors.Select(error => error))
        );

        return Ok(result);
    }
}