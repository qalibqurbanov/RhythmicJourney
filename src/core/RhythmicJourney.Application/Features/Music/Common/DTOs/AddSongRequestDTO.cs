using Microsoft.AspNetCore.Http;

namespace RhythmicJourney.Application.Features.Music.Common.DTOs;

/// <summary>
/// User yeni bir musiqi upload etmek meqsedile bize POST etmiw oldugu datalari temsil edir.
/// </summary>
/// <param name="ArtistName">Musiqici kimdir?</param>
/// <param name="SongName">Musiqinin adi nedir?</param>
/// <param name="SongFile">Musiqi fayli.</param>
/// <param name="SongArt">Musiqinin artwork-u.</param>
public record AddSongRequestDTO(string ArtistName, string SongName, IFormFile SongFile, IFormFile SongArt);