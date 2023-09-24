using RhythmicJourney.Core.Entities.Identity;

namespace RhythmicJourney.Application.Authentication.Abstract;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user);
}