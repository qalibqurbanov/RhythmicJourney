using FluentValidation;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Validators
{
    public class EditRoleCommandValidator : AbstractValidator<EditRoleRequestDTO>
    {
        public EditRoleCommandValidator()
        {
            RuleFor(editRoleRequestDTO => editRoleRequestDTO.newRoleName)
                .MinimumLength(2)
                .NotEmpty();
        }
    }
}