using System.Threading.Tasks;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;

/// <summary>
/// Mail gonderme ile elaqeli emeliyyatlarin imzalarini saxlayir.
/// </summary>
public interface IEmailSender
{
    Task SendConfirmationEmailAsync(AppUser user);
}