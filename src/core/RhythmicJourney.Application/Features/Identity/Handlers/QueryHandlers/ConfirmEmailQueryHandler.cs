using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Extensions;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailQueryHandler(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        if (request.UserID.IsEmpty() || request.ConfirmationToken.IsEmpty())
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.EMAIL_CONFIRM_URL_INVALID } });

        AppUser user = await _userRepository.GetUserByIdAsync(int.Parse(request.UserID));
        {
            if (user == null)
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.EMAIL_CONFIRM_URL_INVALID } });
        }

        // byte[] decodedBytesOfToken = WebEncoders.Base64UrlDecode(request.ConfirmationToken);
        // string token = Encoding.UTF8.GetString(decodedBytesOfToken);

        IdentityResult result = await _userRepository.ConfirmEmailAsync(user, request.ConfirmationToken);
        {
            if (result.Succeeded)
            {
                return await AuthenticationResult.SuccessAsync(RhythmicJourney.Core.Constants.IdentityConstants.EMAIL_CONFIRM_SUCCESSFUL);

            }
            else
            {
                return await AuthenticationResult.FailureAsync(result.Errors.ToList());
            }
        }
    }
}