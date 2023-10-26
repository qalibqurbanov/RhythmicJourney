using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model, CancellationToken cancellationToken)
    {
        RegisterCommand command = new RegisterCommand(model.FirstName, model.LastName, model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join(Environment.NewLine, result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
    {
        LoginQuery query = new LoginQuery(model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join(Environment.NewLine, result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    [HttpDelete("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        LogoutQuery query = new LogoutQuery();
        await _mediator.Send(query);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPost("renew-tokens")]
    public async Task<IActionResult> RenewAccessToken([FromBody] RenewTokensRequestDTO model, CancellationToken cancellationToken)
    {
        RenewTokensCommand command = new RenewTokensCommand(model.RefreshToken);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: RhythmicJourney.Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID
        );

        return Ok(result);
    }

    [HttpGet("confirm-email")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequestDTO model, CancellationToken cancellationToken)
    {
        ConfirmEmailQuery query = new ConfirmEmailQuery(model.UserID, model.ConfirmationToken);
        AuthenticationResult result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO model, CancellationToken cancellationToken)
    {
        ForgotPasswordCommand command = new ForgotPasswordCommand(model.Email);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO model, CancellationToken cancellationToken)
    {
        ResetPasswordCommand command = new ResetPasswordCommand(model.Email, model.Password, model.PasswordConfirm, model.ResetPasswordToken);
        AuthenticationResult result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
        }

        return Ok(result.Message);
    }
}