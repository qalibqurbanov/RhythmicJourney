using System;
using System.Linq;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Repository.Abstract;

namespace RhythmicJourney.Persistence.Repository.Concrete;

/// <summary>
/// Refresh Token ile elaqeli emeliyyatlarin implementasiyalarini saxlayir.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly RhythmicJourneyIdentityDbContext _identityDbContext;
    public RefreshTokenRepository(RhythmicJourneyIdentityDbContext identityDbContext) => this._identityDbContext = identityDbContext;

    public int Add(AppUser user, RefreshToken refreshToken)
    {
        var userFromDb = _identityDbContext.Users.FirstOrDefault(u => u.Id == user.Id);

        if (userFromDb != null)
        {
            { /* Ilk once userin movcud Refresh Tokenlerini revoke/deaktiv edirik: */
                foreach (RefreshToken rt in userFromDb.RefreshTokens)
                {
                    rt.IsActive = false;
                    rt.RevokedOn = DateTime.UtcNow;
                }
            }

            { /* Usere yeni bir Refresh Token veririk. */
                userFromDb.RefreshTokens.Add(refreshToken);
                _identityDbContext.Users.Update(userFromDb);
            }
        }

        return _identityDbContext.SaveChanges();
    }
}