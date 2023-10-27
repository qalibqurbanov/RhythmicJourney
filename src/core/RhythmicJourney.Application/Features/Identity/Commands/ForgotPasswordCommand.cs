using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Wifreni berpa etmek meqsedile edilmiw sorgunu temsil edir.
/// </summary>
public record ForgotPasswordCommand(ForgotPasswordRequestDTO DTO) : IRequest<AuthenticationResult>;