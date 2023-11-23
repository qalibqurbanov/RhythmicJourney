using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

/// <summary>
/// Refresh Token ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IRefreshTokenRepository
{
    void Add(AppUser user, RefreshToken refreshToken);
    void RevokeOldAndExpiredRefreshTokens(AppUser user, string refreshTokenToRevoke);
    void RevokeUsersAllRefreshTokens(int userID);
}