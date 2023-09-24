using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Authentication.Abstract;

namespace RhythmicJourney.Infrastructure.Authentication;

/// <summary>
/// JWT Token generasiya eden funksionalliqlari saxlayan sinif.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration) => this._configuration = configuration;

    /// <summary>
    /// JWT Token generasiya edir.
    /// </summary>
    /// <param name="user">Token yaradan zaman hansi 'user'-in melumatlarindan istifade olunsun.</param>
    /// <returns>Geriye yaradilmiw Tokeni dondurur.</returns>
    public string GenerateToken(AppUser user)
    {
        SigningCredentials signingCredentials = new SigningCredentials
        (
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)),
            algorithm: SecurityAlgorithms.HmacSha256
        );

        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email)
            //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        JwtSecurityToken securityToken = new JwtSecurityToken
        (
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}