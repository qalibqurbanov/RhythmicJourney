using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class RenewTokensCommandValidator : AbstractValidator<RenewTokensCommand>
{
    public RenewTokensCommandValidator()
    {
        RuleFor(renewTokensCommand => renewTokensCommand.DTO.RefreshToken)
            .NotEmpty();
    }
}