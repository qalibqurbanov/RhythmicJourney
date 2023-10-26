﻿namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Clientin sign-up meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datalari temsil edir.
/// </summary>
public record RegisterRequestDTO(string FirstName, string LastName, string Email, string Password);