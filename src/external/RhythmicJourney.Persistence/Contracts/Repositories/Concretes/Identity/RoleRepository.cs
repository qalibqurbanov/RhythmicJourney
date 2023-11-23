using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Persistence.Contracts.Repositories.Concretes.Base;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

namespace RhythmicJourney.Persistence.Contracts.Repositories.Concretes.Identity;

/// <summary>
/// Rollar ile elaqeli emeliyyatlarin implementasiyalarini saxlayir.
/// </summary>
public class RoleRepository : BaseRepository<AppRole, RhythmicJourneyIdentityDbContext>, IRoleRepository
{
    private readonly RhythmicJourneyIdentityDbContext _dbContext;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public RoleRepository(RhythmicJourneyIdentityDbContext dbContext, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) : base(dbContext)
    {
        this._dbContext = dbContext;
        this._roleManager = roleManager;
        this._userManager = userManager;
    }

    #region IRoleRepository
    public async Task<IdentityResult> AddUserToRoleAsync(int UserID, int RoleID)
    {
        AppUser userFromDb = await _userManager.FindByIdAsync(UserID.ToString());
        AppRole roleFromDb = await _roleManager.FindByIdAsync(RoleID.ToString());

        return await _userManager.AddToRoleAsync(userFromDb, roleFromDb.Name);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(string userEmail, string roleName)
    {
        AppUser userFromDb = await _userManager.FindByIdAsync(userEmail);
        {
            return await _userManager.AddToRoleAsync(userFromDb, roleName);
        }
    }

    public async Task<IdentityResult> DeleteUserFromRoleAsync(int UserID, int RoleID)
    {
        AppUser userFromDb = await _userManager.FindByIdAsync(UserID.ToString());
        AppRole roleFromDb = await _roleManager.FindByIdAsync(RoleID.ToString());

        return await _userManager.RemoveFromRoleAsync(userFromDb, roleFromDb.Name);
    }

    public async Task<IdentityResult> DeleteUserFromRoleAsync(string userEmail, string roleName)
    {
        AppUser userFromDb = await _userManager.FindByIdAsync(userEmail);
        {
            return await _userManager.RemoveFromRoleAsync(userFromDb, roleName);
        }
    }

    public IQueryable<AppRole> GetRoles(Expression<Func<AppRole, bool>> expression = null)
    {
        var query = _dbContext.Roles
            .AsNoTracking()
            .AsQueryable();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        return query;
    }

    public AppRole GetRoleById(int RoleID)
    {
        return GetRoles(role => role.Id == RoleID).FirstOrDefault();
    }

    public async Task<AppRole> GetUserRoleAsync(int UserID)
    {
        AppUser userFromDb = _dbContext.Users.FirstOrDefault(user => user.Id == UserID);
        {
            string userRole = (await _userManager.GetRolesAsync(userFromDb))[0];
            AppRole appRole = _roleManager.Roles.FirstOrDefault(role => role.Name == userRole);

            return appRole;
        }
    }

    public async Task<List<AppUser>> GetUsersInRoleAsync(string RoleName)
    {
        return (await _userManager.GetUsersInRoleAsync(RoleName)).ToList();
    }

    public async Task<bool> IsRoleExistsAsync(string RoleName)
    {
        return await _roleManager.RoleExistsAsync(RoleName);
    }

    public async Task<bool> IsRoleExistsAsync(int RoleID)
    {
        return await _roleManager.Roles.AnyAsync(role => role.Id == RoleID);
    }

    public async Task<bool> IsUserInRoleAsync(int UserID, string RoleName)
    {
        AppUser userFromDb = await _userManager.FindByIdAsync(UserID.ToString());
        {
            return await _userManager.IsInRoleAsync(userFromDb, RoleName);
        }
    }
    #endregion IRoleRepository
}