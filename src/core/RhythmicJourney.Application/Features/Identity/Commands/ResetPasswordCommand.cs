using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Wifreni berpa etmek ve ya yeni wifre teyin etmek sorgusunu temsil edir.
/// </summary>
public record ResetPasswordCommand(ResetPasswordRequestDTO DTO) : IRequest<AuthenticationResult>;