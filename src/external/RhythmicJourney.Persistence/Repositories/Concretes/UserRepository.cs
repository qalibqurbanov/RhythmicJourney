using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Persistence.Repositories.Concretes;

/// <summary>
/// Istifadeci/Istifadeciler ile elaqeli funksionalliqlari saxlayir.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly RhythmicJourneyIdentityDbContext _identityDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserRepository(
        RhythmicJourneyIdentityDbContext identityDbContext,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._identityDbContext = identityDbContext;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<AppUser?> GetUserByRefreshToken(string refreshToken)
    {
        return await _identityDbContext.Users
            .Include(t => t.RefreshTokens)
            .FirstOrDefaultAsync(t => t.RefreshTokens.Any(r => r.Token.Equals(refreshToken)));
    }

    public async Task<bool> IsPasswordValid(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<SignInResult> SignIn(string email, string password)
    {
        return await _signInManager.PasswordSignInAsync(email, password, false, false);
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }

    public int Update(AppUser user)
    {
        _identityDbContext.Users.Update(user);

        return _identityDbContext.SaveChanges();
    }
}