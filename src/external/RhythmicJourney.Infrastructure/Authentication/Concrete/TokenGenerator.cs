using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Authentication.Abstract;
using RhythmicJourney.Application.Authentication.Entities;

namespace RhythmicJourney.Infrastructure.Authentication.Concrete;

/// <summary>
/// 'Access Token' ve 'Refresh Token' generasiya eden funksionalliqlarin implementasiyalarini saxlayir.
/// </summary>
public class TokenGenerator : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    public TokenGenerator(JwtSettings jwtSettings) => this._jwtSettings = jwtSettings;

    /// <summary>
    /// Access Token generasiya edir.
    /// </summary>
    /// <param name="user">Token yaradan zaman hansi 'user'-in melumatlarindan istifade olunsun.</param>
    /// <returns>Geriye yaradilmiw Access Tokeni dondurur.</returns>
    public string GenerateAccessToken(AppUser user)
    {
        SigningCredentials signingCredentials = new SigningCredentials
        (
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecretKey)),
            algorithm: SecurityAlgorithms.HmacSha512
        );

        Claim[] claims = new Claim[] /* 'issuer' ve 'audience' claimleri burada yox awagidaki 'JwtSecurityToken' konfiqurasiyasi zamani set edilir. Cunki bu Claim biz elave etmesek bele ozu avtomatik elave olunur Access Tokene. */
        {
            new Claim("UserID", user.Id.ToString()), /* Claimin adi/tipi olaraq custom ad("UserID") demek yerine "ClaimTypes.NameIdentifier"-da demek olardi */
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()), /* Hazirki tarixi UTC-ye cevirib yaziriq */
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        JwtSecurityToken securityToken = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    /// <summary>
    /// Refresh Token yaradir.
    /// </summary>
    /// <returns>Geriye yaradilmiw Refresh Tokeni dondurur.</returns>
    public RefreshToken GenerateRefreshToken()
    {
        SigningCredentials signingCredentials = new SigningCredentials
        (
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecretKey)),
            algorithm: SecurityAlgorithms.HmacSha512
        );

        JwtSecurityToken securityToken = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: null,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
            signingCredentials: signingCredentials
        );

        return RefreshToken.CreateRefreshToken
        (
            refreshToken: new JwtSecurityTokenHandler().WriteToken(securityToken),
            expiresOn: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        );
    }
}