using MediatR;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Queries;

/// <summary>
/// Kateqoriyadaki musiqileri elde etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Uzerinde iw gorulecek kateqoriya.</param>
public record GetCategorySongsQuery(GetCategorySongsRequestDTO DTO) : IRequest<CategoryResult>;