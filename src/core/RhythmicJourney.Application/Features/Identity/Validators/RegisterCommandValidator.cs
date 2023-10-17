using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(registerCommand => registerCommand.FirstName)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.LastName)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.Email)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}