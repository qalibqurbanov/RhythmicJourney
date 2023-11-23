﻿using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Register olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        this._unitOfWork = unitOfWork;
        this._emailSender = emailSender;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UserRepository.GetUserByEmailAsync(request.DTO.Email) is not null)
        {
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.DUPLICATE_EMAIL } });
        }

        AppUser newUser = AppUser.CreateObject(request.DTO.FirstName, request.DTO.LastName, request.DTO.Email, request.DTO.Email);
        {
            /*
                * 'email login' ucun hem 'Email' hem de 'UserEmail' fieldlari uygun email adresi("request.Email") saxlamalidir, bu sebeble her ikisine("Email" ve "UserName") eyni datani("request.Email") verirem.
                + Telefonu login kimi iwletmek isteseydik de eyni wekilde hem "UserName" hem de "Telephone" her ikisi telefon nomremize beraber edecek idik.
            */
        }

        IdentityResult userCreationResult = await _unitOfWork.UserRepository.CreateUserAsync(newUser, request.DTO.Password);
        {
            if (userCreationResult.Succeeded)
            {
                IdentityResult addingUserToRoleResult = await _unitOfWork.RoleRepository.AddUserToRoleAsync(newUser.Email, "USER");
                {
                    if(addingUserToRoleResult.Succeeded)
                    {
                        await _emailSender.SendConfirmationMailAsync(newUser);

                        return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.REGISTER_SUCCESSFUL_AND_CONFIRM_EMAIL);
                    }
                    else
                    {
                        return await AuthenticationResult.FailureAsync(addingUserToRoleResult.Errors.ToList());
                    }
                }
            }
            else
            {
                return await AuthenticationResult.FailureAsync(userCreationResult.Errors.ToList());
            }
        }
    }
}