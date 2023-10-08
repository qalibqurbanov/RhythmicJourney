using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Authentication.Abstract;

/// <summary>
/// 'Access Token' ve 'Refresh Token' generasiya eden funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface ITokenGenerator
{
    string GenerateAccessToken(AppUser user);
    RefreshToken GenerateRefreshToken();
}