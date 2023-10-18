using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;

/// <summary>
/// Mail gonderme ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IEmailSender
{
    Task SendEmailAsync(AppUser user, UserManager<AppUser> userManager, LinkGenerator linkGenerator, HttpContext httpContext);
}