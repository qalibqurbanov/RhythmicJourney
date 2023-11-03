using MediatR;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Commands;

/// <summary>
/// Musiqinin redakte olunmasi sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Userin musiqinin melumatlarini redakte etmek meqsedile bize(servere) POST etdiyi yeni melumatlar.</param>
/// <param name="songIdentityDTO">Hansi musiqinin melumatlari yenilenecek.</param>
public record EditSongCommand(EditSongRequestDTO DTO, SongIdentityDTO songIdentityDTO) : IRequest<SongResult>;