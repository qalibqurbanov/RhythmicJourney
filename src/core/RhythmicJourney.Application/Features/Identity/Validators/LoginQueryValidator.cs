using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Queries;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(loginQuery => loginQuery.Email)
            .NotEmpty();

        RuleFor(loginQuery => loginQuery.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}