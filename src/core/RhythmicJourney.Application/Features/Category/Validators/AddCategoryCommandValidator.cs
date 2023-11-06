using FluentValidation;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Validators;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryRequestDTO>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(addCategoryRequestDTO => addCategoryRequestDTO.CategoryName)
            .MinimumLength(2)
            .NotEmpty();
    }
}