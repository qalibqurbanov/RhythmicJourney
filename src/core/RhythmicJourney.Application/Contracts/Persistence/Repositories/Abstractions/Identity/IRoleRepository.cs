using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Base;

namespace RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Identity;

/// <summary>
/// Rollar ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IRoleRepository : IBaseRepository<AppRole>
{
    Task<IdentityResult> AddUserToRoleAsync(int userID, int roleID);
    Task<IdentityResult> AddUserToRoleAsync(string userEmail, string roleName);
    Task<IdentityResult> DeleteUserFromRoleAsync(int userID, int roleID);
    Task<IdentityResult> DeleteUserFromRoleAsync(string userEmail, string roleName);
    IQueryable<AppRole> GetRoles(Expression<Func<AppRole, bool>> expression = null);
    AppRole GetRoleById(int roleID);
    Task<AppRole> GetUserRoleAsync(int userID);
    Task<List<AppUser>> GetUsersInRoleAsync(string roleName);
    Task<bool> IsRoleExistsAsync(string roleName);
    Task<bool> IsRoleExistsAsync(int roleID);
    Task<bool> IsUserInRoleAsync(int userID, string RoleName);
}