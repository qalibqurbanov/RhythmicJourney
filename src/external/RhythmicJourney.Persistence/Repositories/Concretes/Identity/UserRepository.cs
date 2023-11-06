using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Persistence.Repositories.Concretes.Identity;

/// <summary>
/// Istifadeci/Istifadeciler ile elaqeli emeliyyatlarin saxlayir.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly RhythmicJourneyIdentityDbContext _identityDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserRepository(RhythmicJourneyIdentityDbContext identityDbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        this._identityDbContext = identityDbContext;
    }

    public int Update(AppUser user)
    {
        _identityDbContext.Users.Update(user);

        return _identityDbContext.SaveChanges();
    }

    public async Task<AppUser?> GetUserByIdAsync(int userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _identityDbContext.Users
            .Include(t => t.RefreshTokens)
            .FirstOrDefaultAsync(t => t.RefreshTokens!.Any(r => r.Token.Equals(refreshToken)));
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(AppUser user, string confirmationToken)
    {
        AppUser? userFromDb = await GetUserByIdAsync(user.Id);
        IdentityResult result = await _userManager.ConfirmEmailAsync(userFromDb!, confirmationToken);

        return result;
    }

    public async Task<IdentityResult> ResetPasswordAsync(AppUser user, string resetPasswordToken, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(user, resetPasswordToken, newPassword);
    }

    public async Task<bool> IsPasswordValidAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<SignInResult> SignInAsync(string email, string password)
    {
        return await _signInManager.PasswordSignInAsync(email, password, false, false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}