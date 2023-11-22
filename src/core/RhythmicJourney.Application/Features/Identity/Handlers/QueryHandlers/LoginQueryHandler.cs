using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

/// <summary>
/// Login olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginQueryHandler(ITokenGenerator tokenGenerator, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        this._tokenGenerator = tokenGenerator;
        this._userRepository = userRepository;
        this._refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        AppUser? userFromDb = await _userRepository.GetUserByEmailAsync(request.DTO.Email);
        {
            if (userFromDb is null)
            {
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.USER_NOT_EXISTS } });
            }

            if (!await _userRepository.IsPasswordValidAsync(userFromDb, request.DTO.Password))
            {
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.INVALID_CREDENTIALS } });
            }

            SignInResult result = await _userRepository.SignInAsync(request.DTO.Email, request.DTO.Password);
            {
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.USER_LOCKED } });
                    }
                    else if (result.IsNotAllowed)
                    {
                        if (!userFromDb.EmailConfirmed)
                        {
                            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = RhythmicJourney.Core.Constants.IdentityConstants.EMAIL_NOT_CONFIRMED } });
                        }
                        //else if (!userFromDb.PhoneNumberConfirmed)
                        //{
                        //    return await AuthenticationResult.Failure(new List<IdentityError>() { new IdentityError() { Description = "Your account is not confirmed. Please confirm your phone number to proceed." } });
                        //}
                    }
                    //else if (result.RequiresTwoFactor)
                    //{
                    //    return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = "Please complete the two-factor authentication (2FA) process to access your account." } });
                    //}
                }
            }

        }

        { /* User ucun yeni bir Access ve Refresh Token generate edirik ve dondururuk cliente. Burada hemde generate etdiyimiz hemin bu Refresh Tokeni DB-ya qeyd edirik: */
            RefreshToken RT = _tokenGenerator.GenerateRefreshToken();
            _refreshTokenRepository.Add(userFromDb, RT);

            string newAccessToken = await _tokenGenerator.GenerateAccessTokenAsync(userFromDb);

            return await AuthenticationResult.SuccessAsync(newAccessToken, RT.Token);
        }
    }
}