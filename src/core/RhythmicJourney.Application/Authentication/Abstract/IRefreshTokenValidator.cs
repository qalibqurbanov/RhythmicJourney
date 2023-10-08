using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Authentication.Abstract;

/// <summary>
/// 'Refresh Token'-i validasiyadan kecirmek ucun lazimi funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface IRefreshTokenValidator
{
    bool Validate(string refreshToken);
}