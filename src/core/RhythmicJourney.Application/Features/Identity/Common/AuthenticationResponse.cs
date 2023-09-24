using System;

namespace RhythmicJourney.Application.Features.Identity.Common;

/// <summary>
/// Clientin reallawdirmiw oldugu Authentication emeliyyati neticesinde hemin cliente dondurulecek HTTP Responsu/datalari temsil edir.
/// </summary>
public record AuthenticationResponse(Guid ID, string FirstName, string LastName, string Email, string Token);