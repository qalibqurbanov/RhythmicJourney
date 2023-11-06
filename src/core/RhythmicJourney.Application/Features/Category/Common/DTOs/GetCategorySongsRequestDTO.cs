namespace RhythmicJourney.Application.Features.Category.Common.DTOs;

/// <summary>
/// Kateqoriyadaki musiqileri elde etme sorgusu ucun uzerinde iw gorulecek kateqoriyani temsil edir.
/// </summary>
/// <param name="CategoryID">Uzerinde iw gorulecek kateqoriya.</param>
public record GetCategorySongsRequestDTO(int CategoryID);