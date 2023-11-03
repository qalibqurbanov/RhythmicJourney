using MediatR;
using RhythmicJourney.Application.Features.Music.Common;

namespace RhythmicJourney.Application.Features.Music.Queries;

/// <summary>
/// Musiqileri elde etme sorgusunu temsil edir.
/// </summary>
public record GetSongsQuery : IRequest<SongResult>;