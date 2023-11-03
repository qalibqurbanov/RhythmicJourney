using Microsoft.AspNetCore.Http;

namespace RhythmicJourney.Application.Features.Music.Common.DTOs;

/// <summary>
/// User movcud musiqisini editlemek meqsedile bize post etmiw oldugu datalari temsil edir.
/// </summary>
/// <param name="NewArtistName">Yeni musiqici adi ne olsun?</param>
/// <param name="NewSongName">Yeni musiqi adi ne olsun?</param>
/// <param name="NewSongFile">Yeni musiqi fayli.</param>
/// <param name="NewSongArt">Yeni musiqi artwork-u.</param>
public record EditSongRequestDTO(string? NewArtistName, string? NewSongName, IFormFile? NewSongFile, IFormFile? NewSongArt);