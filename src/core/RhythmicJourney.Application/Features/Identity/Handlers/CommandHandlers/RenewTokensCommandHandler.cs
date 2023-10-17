using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Application.Extensions;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Repository.Abstract;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Authentication.Abstract;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Commands;

namespace RhythmicJourney.Application.Features.Identity.Handlers.CommandHandlers;

/// <summary>
/// Tokenleri yenileme sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class RenewTokensCommandHandler : IRequestHandler<RenewTokensCommand, AuthenticationResult>
{
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public RenewTokensCommandHandler(
        IRefreshTokenValidator refreshTokenValidator,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenGenerator tokenGenerator)
    {
        _refreshTokenValidator = refreshTokenValidator;
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<AuthenticationResult> Handle(RenewTokensCommand request, CancellationToken cancellationToken)
    {
        /*
            1. User 'renew-tokens' endpointimize yeni 'Access Token' ve 'Refresh Token' elde etmek ucun muraciet edir. 
            2. Ilk once bize gonderilmiw 'Refresh Token'-in sahibi olan useri tapiriq.
            3. Ardinca hemin 'Refresh Token'-in butun melumatlarini/sutunlarini elde edirik.
                _
                A. Eger userin gondermiw oldugu hemin bu 'Refresh Token'-in omru qurtaribsa usere bildiririk ki, Refresh Tokenin kecersizdir.
                _
                B. Eger userin gondermiw oldugu hemin bu 'Refresh Token'-in omru qurtarmamiw idise revoke/deaktiv edirik ve ardinca usere yeni bir 'Access Token' ve 'Refresh Token' veririk (ve ardinca ise verdiyimiz hemin bu yeni 'Refresh Token'-de DB-e qeyd edirik).
        */

        if (request.RefreshToken.IsEmpty())
        {
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID } });
        }

        bool isRefreshTokenValid = _refreshTokenValidator.Validate(request.RefreshToken);
        if (!isRefreshTokenValid)
        {
            return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID } });
        }

        AppUser userFromDb = await _userRepository.GetUserByRefreshToken(request.RefreshToken);
        {
            if (userFromDb == null)
            {
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID } });
            }
        }

        RefreshToken existingRefreshToken = userFromDb.RefreshTokens.FirstOrDefault(t => t.Token.Equals(request.RefreshToken));
        {
            if (existingRefreshToken == null)
            {
                return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = Core.Constants.IdentityConstants.REFRESH_TOKEN_INVALID } });
            }
            else
            {
                /* Userin sahib oldugu Refresh Token revoke/deaktiv olunubsa: */
                if (!existingRefreshToken.IsActive || existingRefreshToken.RevokedOn.HasValue || DateTime.UtcNow >= existingRefreshToken.ExpiresOn)
                {
                    return await AuthenticationResult.FailureAsync(new List<IdentityError>() { new IdentityError() { Description = Core.Constants.IdentityConstants.REFRESH_TOKEN_EXPIRED } });
                }

                { /* Ilk once userin hazirki Refresh Tokenini revoke edirik, ardinca ise Usere yeni bir Access ve Refresh Token generate ederek DB-ya qeyd edirik, ardinca ise dondururuk cliente. */
                    _refreshTokenRepository.RevokeOldAndExpiredRefreshTokens(userFromDb, request.RefreshToken);

                    RefreshToken RT = _tokenGenerator.GenerateRefreshToken();
                    _refreshTokenRepository.Add(userFromDb, RT);

                    string newAccessToken = _tokenGenerator.GenerateAccessToken(userFromDb);

                    return await AuthenticationResult.SuccessAsync(newAccessToken, RT.Token);
                }
            }
        }
    }
}