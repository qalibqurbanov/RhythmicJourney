using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

/// <summary>
/// Refresh Token ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IRefreshTokenRepository
{
    int Add(AppUser user, RefreshToken refreshToken);
    int RevokeOldAndExpiredRefreshTokens(AppUser user, string refreshTokenToRevoke);
    int RevokeUsersAllRefreshTokens(int userID);
}