using System;
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
public class AuthenticationController : ControllerBase
{
    public readonly IMediator _mediator;
    public AuthenticationController(IMediator mediator) => this._mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model, CancellationToken cancellationToken)
    {
        RegisterCommand command = new RegisterCommand(model.FirstName, model.LastName, model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(command);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join("\n", result.Error.Select(x => x.Description))
        );

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model, CancellationToken cancellationToken)
    {
        Console.WriteLine(model.Email);
        Console.WriteLine(model.Email);
        Console.WriteLine(model.Password);
        Console.WriteLine(model.Password);
        LoginQuery query = new LoginQuery(model.Email, model.Password);
        AuthenticationResult result = await _mediator.Send(query);

        if (!result.IsSuccess) return Problem
        (
            statusCode: StatusCodes.Status401Unauthorized,
            title: string.Join("\n", result.Error.Select(x => x.Description))
        );

        return Ok(result);
    }
}