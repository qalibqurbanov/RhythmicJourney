namespace RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

/// <summary>
/// 'Refresh Token'-i validasiyadan kecirmek ucun lazimi funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface IRefreshTokenValidator
{
    bool Validate(string refreshToken);
}