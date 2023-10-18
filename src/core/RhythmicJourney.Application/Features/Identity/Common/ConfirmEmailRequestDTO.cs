using System.ComponentModel.DataAnnotations;

namespace RhythmicJourney.Application.Features.Identity.Common;

public class ConfirmEmailRequestDTO
{
    [Required]
    public string UserID { get; set; }

    [Required]
    public string ConfirmationToken { get; set; }
}