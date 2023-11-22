using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Commands;

/// <summary>
/// Yeni rolun elave olunmasi sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Yeni rol yaratmaq meqsedile userin bize(servere) POST etmiw oldugu datalar.</param>
public record AddRoleCommand(AddRoleRequestDTO DTO) : IRequest<RoleResult>;