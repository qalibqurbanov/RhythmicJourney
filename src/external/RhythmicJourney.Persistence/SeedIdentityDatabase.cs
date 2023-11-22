using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace RhythmicJourney.Persistence;

/// <summary>
/// Proyekt sifirdan ayaga qaldirilanda DB bow olacaq, lakin biz gerek en azi 1 admin icazeli user DB-miza elave edek ki, xususi icaze teleb eden menecment kimi sehifelere giriw icazesi olan azi 1 user olmuw olsun elimizde. Bu sinif ozunde app ayaga qaldirilan vaxti yaradilmasi lazim olan User ve User Rollarini saxlayir.
/// </summary>
public static class SeedIdentityDatabase
{
    /// <summary>
    /// App ayaga qaldirilanda DB-da yaradilmasi lazim olan User ve User Rollarini yaradir.
    /// </summary>
    public async static Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            RhythmicJourneyIdentityDbContext _dbContext = scope.ServiceProvider.GetService<RhythmicJourneyIdentityDbContext>();
            {
                RoleManager<AppRole> _roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();
                {
                    string[] Roles = new string[] { "GUEST", "USER", "MODERATOR", "ADMIN" };

                    foreach (string roleName in Roles)
                    {
                        if (!_dbContext.Roles.Any(role => role.Name == roleName))
                        {
                            await _roleManager.CreateAsync(new AppRole() { Name = roleName, NormalizedName = roleName.ToUpper() });
                        }
                    }
                }

                UserManager<AppUser> _userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
                {
                    string login = "admin123@gmail.com";
                    string passw = "Admin_123";

                    if (!_dbContext.Users.Any(user => user.Email == login))
                    {
                        PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
                        {
                            AppUser user_Admin = new AppUser()
                            {
                                FirstName = "Lorem", LastName = "Ipsum",
                                UserName = login, NormalizedUserName = login.ToUpper(),
                                Email = login, NormalizedEmail = login.ToUpper(), EmailConfirmed = true,
                                PasswordHash = passwordHasher.HashPassword(null, passw)
                            };

                            await _userManager.CreateAsync(user_Admin);

                            {
                                string[] Roles = _dbContext.Roles.Select(role => role.Name).ToArray();

                                var result = await _userManager.AddToRolesAsync(user_Admin, Roles);
                            }
                        }
                    }
                }
            }
        }
    }
}