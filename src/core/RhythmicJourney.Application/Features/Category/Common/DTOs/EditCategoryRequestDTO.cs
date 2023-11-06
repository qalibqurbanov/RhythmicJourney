namespace RhythmicJourney.Application.Features.Category.Common.DTOs;

/// <summary>
/// Kateqoriyani redakte etmek ucun gonderilmiw yeni melumatlari temsil edir.
/// </summary>
/// <param name="NewCategoryName">Kateqoriyanin yeni adi ne olsun?</param>
public record EditCategoryRequestDTO(string NewCategoryName);