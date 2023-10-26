using System.ComponentModel.DataAnnotations;

namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Client ozunun akkauntunu email vasitesile tesdiqlemek meqsedile bize gondermiw oldugu datalari temsil edir.
/// </summary>
public record ConfirmEmailRequestDTO(string UserID, string ConfirmationToken);