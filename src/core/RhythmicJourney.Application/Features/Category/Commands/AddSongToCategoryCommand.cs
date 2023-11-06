using MediatR;
using RhythmicJourney.Application.Features.Category.Common;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Commands;

/// <summary>
/// Kateqoriyaya musiqi elave etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Hansi musiqi hansi kateqoriyaya elave olunacaq?</param>
public record AddSongToCategoryCommand(AddSongToCategoryRequestDTO DTO) : IRequest<CategoryResult>;