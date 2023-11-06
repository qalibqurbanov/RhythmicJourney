using FluentValidation;
using RhythmicJourney.Application.Features.Category.Common.DTOs;

namespace RhythmicJourney.Application.Features.Category.Validators;

public class AddSongToCategoryCommandValidator : AbstractValidator<AddSongToCategoryRequestDTO>
{
    public AddSongToCategoryCommandValidator()
    {
        RuleFor(addSongToCategoryRequestDTO => addSongToCategoryRequestDTO.SongID)
            .GreaterThan(-1)
            .NotEmpty();

        RuleFor(addSongToCategoryRequestDTO => addSongToCategoryRequestDTO.CategoryID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}