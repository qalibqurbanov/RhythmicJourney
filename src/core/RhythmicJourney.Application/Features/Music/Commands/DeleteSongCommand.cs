using MediatR;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Commands;

/// <summary>
/// Musiqinin sistemden silinmesini sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Hansi musiqinin silinmeyini isteyirik?</param>
public record DeleteSongCommand(SongIdentityDTO DTO) : IRequest<SongResult>;