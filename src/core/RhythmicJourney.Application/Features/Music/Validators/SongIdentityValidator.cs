using FluentValidation;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Validators;

public class SongIdentityValidator : AbstractValidator<SongIdentityDTO>
{
    public SongIdentityValidator()
    {
        RuleFor(songIdentityDTO => songIdentityDTO.SongID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}