using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Queries;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
{
    public ConfirmEmailQueryValidator()
    {
        RuleFor(confirmEmailQuery => confirmEmailQuery.DTO.UserID)
            .NotEmpty()
            .GreaterThan("-1");

        RuleFor(confirmEmailQuery => confirmEmailQuery.DTO.ConfirmationToken)
            .NotEmpty();
    }
}