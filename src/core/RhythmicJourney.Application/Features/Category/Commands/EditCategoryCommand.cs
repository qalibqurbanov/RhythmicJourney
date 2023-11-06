using MediatR;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Commands;

/// <summary>
/// Kateqoriyani redakte etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Redakte olunacaq kateqoriyanin yeni melumatlari.</param>
/// <param name="categoryIdentityDTO">Redakte olunacaq kateqoriya.</param>
public record EditCategoryCommand(EditCategoryRequestDTO DTO, CategoryIdentityDTO categoryIdentityDTO) : IRequest<CategoryResult>;