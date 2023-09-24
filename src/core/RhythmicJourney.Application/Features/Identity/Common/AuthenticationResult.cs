using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Features.Identity.Common;

/// <summary>
/// Authentication emeliyyatinin neticesini dondurur (+ Bu sinif 'Result Object Design Pattern'-in implementasiyasidir).
/// </summary>
public class AuthenticationResult
{
    public AppUser User { get; private set; }
    public string Token { get; private set; }

    public bool IsSuccess { get; private set; }
    public List<IdentityError> Error { get; private set; } = null!;

    private AuthenticationResult() { /* Awagidaki spesifik neticeleri temsil eden metodlarin cagirilmasini isteyirem deye bu konstruktoru 'private' vasitesile gizledirem */ }

    public static Task<AuthenticationResult> Success(AppUser user, string token) => Task.FromResult(new AuthenticationResult() { IsSuccess = true, User = user, Token = token });
    public static Task<AuthenticationResult> Failure(List<IdentityError> error) => Task.FromResult(new AuthenticationResult() { IsSuccess = false, Error = error });
}