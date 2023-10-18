using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RhythmicJourney.Application.Models;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

namespace RhythmicJourney.Infrastructure.Identity.Concretes;

/// <summary>
/// 'Refresh Token'-i validasiyadan kecirmek ucun lazimi emeliyyatlarin implementasiyalarini saxlayir.
/// <br/> <br/>
/// 'Refresh Token'-i validasiya etmek? 'Refresh Tokeni' eyni 'Access Token'-in yaradildigi kimi yaratmiwam, yeni ('Access Token' yaratmaqdan ferqli olaraq 'Refresh Token' yaradan zaman userin claimlerini yerlewdirmirem) mueyyen bir 'Secret Key' vasitesile tokeni wifreleyirem ve wifrelenmiw bu 'Refresh Token'-i verirem usere. Netice olaraq JWT kitabxanasi (avtomatik bir wekilde) 'Access Token'-i her bir requestde validasiya edirse, burada eyni validasiyani men manual bir wekilde 'Refresh Token'-e tetbiq edirem.
/// </summary>
public class RefreshTokenValidator : IRefreshTokenValidator
{
    private readonly JwtSettings _jwtSettings;
    public RefreshTokenValidator(JwtSettings jwtSettings) => this._jwtSettings = jwtSettings;

    public bool Validate(string refreshToken)
    {
        JwtSecurityTokenHandler refreshTokenHandler = new JwtSecurityTokenHandler();

        TokenValidationParameters refreshTokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecretKey)),
        };

        try
        {
            refreshTokenHandler.ValidateToken(refreshToken, refreshTokenValidationParameters, out var validatedRefreshToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}