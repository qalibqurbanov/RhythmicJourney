using System;

namespace RhythmicJourney.Core.Exceptions;

/// <summary>
/// Bu exception, userin daxil etmiw oldugu log-in melumatlarinin yanliw oldugu halda throw edilir.
/// </summary>
public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() { }

    public InvalidCredentialsException(string? message = "Invalid credentials, please double-check your username and password and try again.") : base(message) { }
}