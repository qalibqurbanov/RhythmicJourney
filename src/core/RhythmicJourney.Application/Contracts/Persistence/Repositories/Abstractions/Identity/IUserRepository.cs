﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

/// <summary>
/// Istifadeciler ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IUserRepository
{
    void Update(AppUser user);
    Task<AppUser?> GetUserByIdAsync(int userId);
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<IdentityResult> ConfirmEmailAsync(AppUser user, string confirmationToken);
    Task<IdentityResult> ResetPasswordAsync(AppUser user, string resetPasswordToken, string newPassword);
    Task<SignInResult> SignInAsync(string email, string password);
    Task SignOutAsync();
    Task<bool> IsPasswordValidAsync(AppUser user, string password);
    Task<bool> IsUserExistsAsync(int UserID);
}