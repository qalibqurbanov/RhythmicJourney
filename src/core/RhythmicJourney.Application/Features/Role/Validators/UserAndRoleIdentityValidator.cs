using FluentValidation;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Validators;

public class UserAndRoleIdentityValidator : AbstractValidator<UserAndRoleIdentityDTO>
{
    public UserAndRoleIdentityValidator()
    {
        RuleFor(userAndRoleIdentityDTO => userAndRoleIdentityDTO.UserID)
            .GreaterThan(-1)
            .NotEmpty();

        RuleFor(userAndRoleIdentityDTO => userAndRoleIdentityDTO.RoleID)
            .GreaterThan(-1)
            .NotEmpty();
    }
}