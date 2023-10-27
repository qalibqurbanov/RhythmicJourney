using MediatR;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

/// <summary>
/// Logout olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class LogoutQueryHandler : IRequestHandler<LogoutQuery>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        this._httpContextAccessor = httpContextAccessor;
        this._userRepository = userRepository;
        this._refreshTokenRepository = refreshTokenRepository;
    }

    public async Task Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID"), out int userID))
        {
            return;
        }

        {
            _refreshTokenRepository.RevokeUsersAllRefreshTokens(userID);
            await _userRepository.SignOutAsync();
        }
    }
}