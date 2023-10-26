using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Wifreni berpa etmek ve ya yeni wifre teyin etmeyi reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    public ResetPasswordCommandHandler(IUserRepository userRepository) => this._userRepository = userRepository;

    public async Task<AuthenticationResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser userFromDb = await _userRepository.GetUserByEmailAsync(request.Email);
        {
            if(userFromDb != null)
            {
                IdentityResult result = await _userRepository.ResetPasswordAsync(userFromDb, request.ResetPasswordToken, request.Password);

                if (result.Succeeded)
                {
                    return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.PASSWORD_RESET_SUCCESSFUL);
                }
                else
                {
                    return await AuthenticationResult.FailureAsync(result.Errors.ToList());
                }
            }
            else
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.USER_NOT_EXISTS } });
        }
    }
}