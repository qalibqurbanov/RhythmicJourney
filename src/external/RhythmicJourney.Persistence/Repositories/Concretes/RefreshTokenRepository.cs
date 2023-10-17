using System;
using System.Linq;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Persistence.Repositories.Concretes;

/// <summary>
/// Refresh Token ile elaqeli emeliyyatlarin implementasiyalarini saxlayir.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly RhythmicJourneyIdentityDbContext _identityDbContext;
    public RefreshTokenRepository(RhythmicJourneyIdentityDbContext identityDbContext) => this._identityDbContext = identityDbContext;

    public int Add(AppUser user, RefreshToken refreshToken)
    {
        AppUser userFromDb = _identityDbContext.Users.FirstOrDefault(u => u.Id == user.Id);

        if (userFromDb != null)
        {
            userFromDb.RefreshTokens.Add(refreshToken);
        }

        return _identityDbContext.SaveChanges();
    }

    public int RevokeOldAndExpiredRefreshTokens(AppUser user, string refreshTokenToRevoke)
    {
        /* Ilk once uzerinde iw goreceyimiz useri elde edirik: */
        AppUser userFromDb = _identityDbContext.Users.FirstOrDefault(u => u.Id == user.Id);

        { /* "/renew-tokens" endpointimize hansi Refresh Token ile request edilmiwdise hemin Refresh Tokeni revoke/deaktiv edirik: */
            RefreshToken RT = userFromDb.RefreshTokens.FirstOrDefault(u => u.Token == refreshTokenToRevoke);
            {
                RT.IsActive = false;
                RT.RevokedOn = DateTime.UtcNow;
            }
        }

        { /* Userin her ugurlu logini zamani usere Access Token-le yanawi hem de yeni bir Refresh Token verirem ve bu Refresh Tokenler bir muddet sonra DB-da yigilib qalacaq. Bu sebeble omru qurtarmiw Refresh Tokenleri ilk once burada deaktiv edirem (+ lakin daha sonra mueyyen intervalla DB-da olu Refresh Tokenleri temizlemek lazimdir). */
            foreach (RefreshToken RT in userFromDb.RefreshTokens)
            {
                if (DateTime.UtcNow >= RT.ExpiresOn)
                {
                    RT.IsActive = false;
                    RT.RevokedOn = DateTime.UtcNow;
                }
            }
        }

        return _identityDbContext.SaveChanges();
    }

    public int RevokeUsersAllRefreshTokens(int userID)
    {
        AppUser userFromDb = _identityDbContext.Users
            .Include(user => user.RefreshTokens)
            .FirstOrDefault(u => u.Id == userID);

        if (userFromDb != null)
        {
            foreach(RefreshToken RT in userFromDb.RefreshTokens)
            {
                RT.IsActive = false;
                RT.RevokedOn = DateTime.UtcNow;
            }
        }

        return _identityDbContext.SaveChanges();
    }
}