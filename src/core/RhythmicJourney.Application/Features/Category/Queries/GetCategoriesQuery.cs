using MediatR;
using RhythmicJourney.Application.Features.Category.Common;

namespace RhythmicJourney.Application.Features.Category.Queries;

/// <summary>
/// Kateqoriyalari elde etme sorgusunu temsil edir.
/// </summary>
public record GetCategoriesQuery : IRequest<CategoryResult>;