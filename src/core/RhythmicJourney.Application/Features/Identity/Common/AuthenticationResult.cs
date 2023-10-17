using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RhythmicJourney.Application.Features.Identity.Common;

/// <summary>
/// User autentifikasiyadan ugurlu/ugursuz kecdikden sonra usere ('Result Object Design Pattern'-in implementasiyasi olan) bu sinif vasitesile uygun neticeni dondururuk.
/// </summary>
public partial class AuthenticationResult
{
    private AuthenticationResult() { /* Awagidaki spesifik neticeleri temsil eden metodlarin cagirilmasini isteyirem deye bu konstruktoru 'private' vasitesile gizledirem */ }

    /// <summary>
    /// Usere netice olaraq Access ve Refresh token dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<AuthenticationResult> SuccessAsync(string accessToken, string refreshToken) => Task.FromResult(new AuthenticationResult() { IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken });

    /// <summary>
    /// Usere netice olaraq sadece mesaj dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<AuthenticationResult> SuccessAsync(string message) => Task.FromResult(new AuthenticationResult() { Message = message, IsSuccess = true });

    /// <summary>
    /// Usere netice olaraq baw vermiw xetani dondurmek isteyirikse bu overloadi iwledirik.
    /// </summary>
    public static Task<AuthenticationResult> FailureAsync(List<IdentityError> error) => Task.FromResult(new AuthenticationResult() { IsSuccess = false, Errors = error });
}

public partial class AuthenticationResult
{
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }

    public string Message { get; set; }

    public bool IsSuccess { get; private set; }
    public List<IdentityError> Errors { get; private set; }
}