using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

/// <summary>
/// Istifadeci/Istifadeciler ile elaqeli funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface IUserRepository
{
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<AppUser?> GetUserByRefreshToken(string refreshToken);
    Task<SignInResult> SignIn(string email, string password);
    Task SignOut();
    Task<bool> IsPasswordValid(AppUser user, string password);
    int Update(AppUser user);
}