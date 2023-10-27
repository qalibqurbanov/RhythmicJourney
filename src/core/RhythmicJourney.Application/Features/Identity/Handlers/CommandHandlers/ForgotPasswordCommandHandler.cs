using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Wifreni berpa etmek meqsedile ilkin merheleni(userin mail adresini almaq ve hemin maile wifrenin berpasi ucun link gondermek) reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(IEmailSender emailSender, IUserRepository userRepository)
    {
        this._userRepository = userRepository;
        this._emailSender = emailSender;
    }

    public async Task<AuthenticationResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? userFromDb = await _userRepository.GetUserByEmailAsync(request.DTO.Email);
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