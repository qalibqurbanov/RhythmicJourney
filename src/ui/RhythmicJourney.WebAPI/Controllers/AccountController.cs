using System;
using MediatR;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Features.Identity.Commands;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator) => this._mediator = mediator;

    /// <summary>
    /// Qeydiyyatdan kecmek ucundur.
    /// </summary>
    /// <remarks>
    /// Numune request:
    /// 
    ///     POST api/register
    ///     {
    ///         "FirstName": "Filankes",
    ///         "LastName": "Filankesov",
    ///         "Email": "filankes_filankesov@test.com",
    ///         "Password": "Filankes123!"
    ///     }
    /// </remarks>
    /// <param name="model">Qeydiyyyatdan kecmek meqsedile userin daxil etdiyi melumatlar.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Qeydiyyatdan kecmek ucundur.", Description = "Bu endpoint vasitesile yeni bir user yaradilir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model, CancellationToken cancellationToken)
    {
        RegisterCommand command = new RegisterCommand(model);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join(Environment.NewLine, result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    /// <summary>
    /// Login olmaq ucundur.
    /// </summary>
    /// <remarks>
    /// Numune request:
    /// 
    ///     POST api/login
    ///     {
    ///         "Email": "filankes_filankesov@test.com",
    ///         "Password": "Filankes123!"
    ///     }
    /// </remarks>
    /// <param name="model">Login olmaq meqsedile userin daxil etdiyi giriw melumatlari(login/pass).</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login olmaq ucundur.", Description = "Bu endpoint vasitesile movcud user profiline giriw edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
    {
        LoginQuery query = new LoginQuery(model);
        AuthenticationResult result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join(Environment.NewLine, result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    /// <summary>
    /// Logout olmaq ucundur.
    /// </summary>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye hecne dondurmur.</returns>
    [HttpDelete("logout")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout olmaq ucundur.", Description = "Bu endpoint vasitesile user profilinden cixiw edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status403Forbidden)]
    [Consumes("application/json")]
    [Produces(typeof(void))]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        LogoutQuery query = new LogoutQuery();
        await _mediator.Send(query, cancellationToken);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Yeni bir Access ve Refresh Token almaq ucundur.
    /// </summary>
    /// <param name="model">User yeni bir Access ve Refresh Token almagi meqsedile gonderdiyi kecerli bir Refresh Token.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("renew-tokens")]
    [SwaggerOperation(Summary = "Yeni bir Access ve Refresh Token almaq ucundur.", Description = "Bu endpoint vasitesile user yeni bir Access ve Refresh Token elde edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RenewAccessToken([FromBody] RenewTokensRequestDTO model, CancellationToken cancellationToken)
    {
        RenewTokensCommand command = new RenewTokensCommand(model);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: RhythmicJourney.Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID
        );

        return Ok(result);
    }

    /// <summary>
    /// Userin mailini tesdiqlemek ucundur.
    /// </summary>
    /// <param name="model">User mailini tesdiqlemesi meqsedile gonderdiyi melumatlar(kecerli bir mail tesdiqleme tokeni ve maili tesdiqlenecek user).</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpGet("confirm-email")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [SwaggerOperation(Summary = "Userin mailini tesdiqlemek ucundur.", Description = "Bu endpoint vasitesile user akauntunun mailini tesdiqledir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequestDTO model, CancellationToken cancellationToken)
    {
        ConfirmEmailQuery query = new ConfirmEmailQuery(model);
        AuthenticationResult result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }

    /// <summary>
    /// Wifrenin berpasi ucun mail gondermek ucundur.
    /// </summary>
    /// <param name="model">Wifrenin berpasi meqsedile userin daxil etdiyi mail.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("forgot-password")]
    [SwaggerOperation(Summary = "Wifrenin berpasi ucun mail gondermek ucundur.", Description = "Bu endpoint vasitesile user akauntuna giriwi berpa edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO model, CancellationToken cancellationToken)
    {
        ForgotPasswordCommand command = new ForgotPasswordCommand(model);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }

    /// <summary>
    /// Yeni wifre teyin etmek ucundur.
    /// </summary>
    /// <param name="model">Yeni wifre teyin etmek meqsedile userin daxil etdiyi yeni wifre.</param>
    /// <param name="cancellationToken">Emeliyyati legv etmek ucundur.</param>
    /// <returns>Geriye emeliyyatin nece yekunlawdigi ile elaqeli melumatlari ozunde saxlayan 'Result Object Design Pattern' implementasiyasi olan data dondurur.</returns>
    [HttpPost("reset-password")]
    [SwaggerOperation(Summary = "Yeni wifre teyin etmek ucundur.", Description = "Bu endpoint vasitesile user akauntuna yeni wifre teyin edir.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO model, CancellationToken cancellationToken)
    {
        ResetPasswordCommand command = new ResetPasswordCommand(model);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }
}