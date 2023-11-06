using MediatR;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Commands;

/// <summary>
/// Kateqoriyani silme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Uzerinde iw gorulecek kateqoriya.</param>
public record DeleteCategoryCommand(CategoryIdentityDTO DTO) : IRequest<CategoryResult>;