using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator) => this._mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO model, CancellationToken cancellationToken)
    {
        RegisterCommand command = new RegisterCommand(model.FirstName, model.LastName, model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(command);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join("\n", result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO model, CancellationToken cancellationToken)
    {
        LoginQuery query = new LoginQuery(model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(query);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join("\n", result.Errors.Select(x => x.Description))
        );

        return Ok(result);
    }

    [HttpPost("renew-tokens")]
    public async Task<IActionResult> RenewAccessToken([FromBody] RenewTokensRequestDTO model, CancellationToken cancellationToken)
    {
        RenewTokensCommand renewTokensQuery = new RenewTokensCommand(model.RefreshToken);
        AuthenticationResult result = await _mediator.Send(renewTokensQuery);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: RhythmicJourney.Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID
        );

        return Ok(result);
    }
}