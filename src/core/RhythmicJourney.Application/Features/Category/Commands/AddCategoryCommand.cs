using MediatR;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Commands;

/// <summary>
/// Kateqoriya yaratma sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Kateqoriyanin melumatlari.</param>
public record AddCategoryCommand(AddCategoryRequestDTO DTO) : IRequest<CategoryResult>;