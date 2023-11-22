using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RhythmicJourney.Application.Models;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

namespace RhythmicJourney.Infrastructure.Identity.Concretes;

/// <summary>
/// 'Access Token' ve 'Refresh Token' generasiya eden emeliyyatlarin implementasiyalarini saxlayir.
/// </summary>
public class TokenGenerator : ITokenGenerator
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public TokenGenerator(JwtSettings jwtSettings, UserManager<AppUser> userManager)
    {
        this._jwtSettings = jwtSettings;
        this._userManager = userManager;
    }

    /// <summary>
    /// Access Token generasiya edir.
    /// </summary>
    /// <param name="user">Token yaradan zaman hansi 'user'-in melumatlarindan istifade olunsun.</param>
    /// <returns>Geriye yaradilmiw Access Tokeni dondurur.</returns>
    public async Task<string> GenerateAccessTokenAsync(AppUser user)
    {
        // Tokeni nece imzalayacagimi hazirlayiram:
        SigningCredentials signingCredentials = new SigningCredentials
        (
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecretKey)),
            algorithm: SecurityAlgorithms.HmacSha512
        );

        // Usere vereceyim claimleri yaradiram:
        List<Claim> claims = new List<Claim>()
        {
            /* 'issuer' ve 'audience' claimleri burada yox awagidaki 'JwtSecurityToken' konfiqurasiyasi zamani set edilir. Cunki bu Claim biz elave etmesek bele ozu avtomatik elave olunur Access Tokene. */

            new Claim("UserID", user.Id.ToString()), /* Claimin adi/tipi olaraq custom ad("UserID") demek yerine "ClaimTypes.NameIdentifier"-da demek olardi */
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, "USER") /* Login olmuw her bir userin Tokeninde 'User' claimi olsun. Cunki API-daki endpointlere muracieti 'User' ve ya onun uzerindekiler ede biler (Account endpointlerinden bawqa) */
        };

        // Ardinca userin evvelceden sahib olmuw oldugu claimleri DB-dan elde ederek elave edirem claimleri saxlayan kolleksiyama:
        {
            var existingClaimsOfUser = await _userManager.GetClaimsAsync(user);
            claims.AddRange(existingClaimsOfUser);
        }

        // Daha sonra ise userin rollarini DB-dan elde edirem ve hemin rollari claim icerisine yerlewdirerek elave edirem claimleri saxlayan kolleksiyama:
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (string roleName in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }
        }

        // Yaradacagim tokeni konfiqurasiya edirem:
        JwtSecurityToken securityToken = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
            signingCredentials: signingCredentials
        );

        // Tokeni yaradiram:
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

        return RefreshToken.CreateObject
        (
            refreshToken: new JwtSecurityTokenHandler().WriteToken(securityToken),
            expiresOn: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        );
    }
}