#pragma warning disable RCS1194

using System;

namespace RhythmicJourney.Core.Exceptions;

/// <summary>
/// Bu exception, user sistemimizde tapilmadigi halda throw edilir.
/// </summary>
public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException() { }

    public UserNotFoundException(string? message = "The user with this email does not exist in our system, please double-check your email and try logging in again.") : base(message) { }
}