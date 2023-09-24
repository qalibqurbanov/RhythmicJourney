using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Persistence.Repository.Abstract;

namespace RhythmicJourney.Persistence.Repository.Concrete;

/// <summary>
/// Istifadeci/Istifadeciler ile elaqeli funksionalliqlari saxlayir.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<SignInResult> SignIn(string email, string password)
    {
        return await _signInManager.PasswordSignInAsync(email, password, false, false);
    }
}