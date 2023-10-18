﻿using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(registerCommand => registerCommand.FirstName)
            .Length(3, 20)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.LastName)
            .Length(2, 40)
            .NotEmpty();

        RuleFor(registerCommand => registerCommand.Email)
            .NotEmpty()
            .Matches("(?:[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])");

        RuleFor(registerCommand => registerCommand.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}