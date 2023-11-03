using FluentValidation;
using RhythmicJourney.Application.Features.Music.Common.DTOs;

namespace RhythmicJourney.Application.Features.Music.Validators;

public class AddSongCommandValidator : AbstractValidator<AddSongRequestDTO>
{
    public AddSongCommandValidator()
    {
        RuleFor(addSongRequestDTO => addSongRequestDTO.ArtistName)
            .NotEmpty();

        RuleFor(addSongRequestDTO => addSongRequestDTO.SongName)
            .NotEmpty();

        RuleFor(addSongRequestDTO => addSongRequestDTO.SongFile)
            .NotEmpty();

        RuleFor(addSongRequestDTO => addSongRequestDTO.SongArt)
            .NotEmpty();
    }
}