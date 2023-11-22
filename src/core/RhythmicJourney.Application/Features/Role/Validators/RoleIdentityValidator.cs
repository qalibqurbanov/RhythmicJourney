using FluentValidation;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Validators;

public class RoleIdentityValidator : AbstractValidator<RoleIdentityDTO>
{
    public RoleIdentityValidator()
    {
        RuleFor(roleIdentityDTO => roleIdentityDTO.RoleID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}