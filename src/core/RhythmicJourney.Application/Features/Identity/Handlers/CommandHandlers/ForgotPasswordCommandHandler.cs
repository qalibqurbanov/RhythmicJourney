using MediatR;
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
/// Wifreni berpa etmek meqsedile ilkin merheleni(userin mail adresini almaq ve hemin maile wifrenin berpasi ucun link gondermek) reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, AuthenticationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        this._unitOfWork = unitOfWork;
        this._emailSender = emailSender;
    }

    public async Task<AuthenticationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? userFromDb = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.DTO.Email);
        {
            if (userFromDb is null)
            {
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.USER_NOT_EXISTS } });
            }
        }

        {
            await _emailSender.SendResetPasswordMailAsync(userFromDb);
        }

        return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.EMAIL_RESET_SENT);
    }
}