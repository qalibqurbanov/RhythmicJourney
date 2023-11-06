using FluentValidation;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Validators;

public class GetCategorySongsQueryValidator : AbstractValidator<GetCategorySongsRequestDTO>
{
    public GetCategorySongsQueryValidator()
    {
        RuleFor(getSongsByCategoryRequestDTO => getSongsByCategoryRequestDTO.CategoryID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}