using FluentValidation;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Validators;

public class AddRoleCommandValidator : AbstractValidator<AddRoleRequestDTO>
{
    public AddRoleCommandValidator()
    {
        RuleFor(addRoleRequestDTO => addRoleRequestDTO.RoleName)
            .MinimumLength(2)
            .NotEmpty();
    }
}