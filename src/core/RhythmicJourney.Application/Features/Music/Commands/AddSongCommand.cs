using MediatR;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Commands;

/// <summary>
/// Yeni musiqi elave etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Yeni musiqi upload etmek meqsedile userin bize(servere) POST etmiw oldugu datalar.</param>
public record AddSongCommand(AddSongRequestDTO DTO) : IRequest<SongResult>;