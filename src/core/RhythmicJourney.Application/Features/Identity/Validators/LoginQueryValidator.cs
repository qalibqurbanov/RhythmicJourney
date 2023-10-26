﻿using FluentValidation;
using RhythmicJourney.Application.Features.Identity.Queries;

namespace RhythmicJourney.Application.Features.Identity.Validators;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(loginQuery => loginQuery.Email)
            .NotEmpty()
            .Matches(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z]{2,})$").WithMessage("Oops! It seems you provided an invalid email address. Valid formats are: 'test@hostname.com', 'te.st@hostname.com', 'te_st@hostname.com', 'te-st@hostname.com', 'te-st@hostname.com.edu', 'te-s.t@hostname.com.edu', 't.e.s.t@hostname.com.edu', 'test1337@hostname.com', '1337@hostname.com', '1-3.3-7@hostname.com'");

        RuleFor(loginQuery => loginQuery.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}