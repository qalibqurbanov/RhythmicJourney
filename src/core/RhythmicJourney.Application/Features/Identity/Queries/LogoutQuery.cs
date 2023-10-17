using MediatR;

namespace RhythmicJourney.Application.Features.Identity.Queries;

/// <summary>
/// Logout sorgusunu temsil edir.
/// </summary>
public record LogoutQuery() : IRequest;