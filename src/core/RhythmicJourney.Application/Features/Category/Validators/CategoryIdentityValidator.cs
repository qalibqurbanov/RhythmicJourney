using FluentValidation;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Validators;

public class CategoryIdentityValidator : AbstractValidator<CategoryIdentityDTO>
{
    public CategoryIdentityValidator()
    {
        RuleFor(categoryIdentityDTO => categoryIdentityDTO.CategoryID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}