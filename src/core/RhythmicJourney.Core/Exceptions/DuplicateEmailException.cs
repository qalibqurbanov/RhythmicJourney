#pragma warning disable RCS1194

using System;

namespace RhythmicJourney.Core.Exceptions;

/// <summary>
/// Bu exception, userin daxil etdiyi email adresi sistemimizde artiq movcuddursa throw edilir.
/// </summary>
public sealed class DuplicateEmailException : Exception
{
    public DuplicateEmailException() { }

    public DuplicateEmailException(string? message = "Account with this email already exists.") : base(message) { }
}