using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Qeydiyyatdan kecme sorgusunu temsil edir.
/// </summary>
public record RegisterCommand(RegisterRequestDTO DTO) : IRequest<AuthenticationResult>;