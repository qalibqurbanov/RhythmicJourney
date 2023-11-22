﻿using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

/// <summary>
/// 'Access Token' ve 'Refresh Token' generasiya eden emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface ITokenGenerator
{
    Task<string> GenerateAccessTokenAsync(AppUser user);
    RefreshToken GenerateRefreshToken();
}