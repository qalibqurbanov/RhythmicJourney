using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Persistence.Repository.Abstract;

/// <summary>
/// Istifadeci/Istifadeciler ile elaqeli funksionalliqlarin imzalarini saxlayir.
/// </summary>
public interface IUserRepository
{
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<AppUser?> GetUserByRefreshToken(string refreshToken);
    int Update(AppUser user);
}