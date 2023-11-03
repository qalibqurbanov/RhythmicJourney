using FluentValidation;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Validators;

public class EditSongCommandValidator : AbstractValidator<EditSongRequestDTO>
{
    public EditSongCommandValidator()
    {
        RuleFor(editSongRequestDTO => editSongRequestDTO.NewArtistName)
        .NotEmpty();

        RuleFor(editSongRequestDTO => editSongRequestDTO.NewSongName)
            .NotEmpty();

        RuleFor(editSongRequestDTO => editSongRequestDTO.NewSongFile)
            .NotEmpty();

        RuleFor(editSongRequestDTO => editSongRequestDTO.NewSongArt)
            .NotEmpty();
    }
}