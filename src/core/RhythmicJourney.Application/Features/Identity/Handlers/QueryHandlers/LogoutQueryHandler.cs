using MediatR;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Application.Features.Identity.Queries;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

/// <summary>
/// Logout olma sorgusunu reallawdiracaq olan funksionalligi saxlayir.
/// </summary>
public class LogoutQueryHandler : IRequestHandler<LogoutQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        this._unitOfWork = unitOfWork;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID"), out int userID))
        {
            return;
        }

        {
            _unitOfWork.RefreshTokenRepository.RevokeUsersAllRefreshTokens(userID);
            await _unitOfWork.UserRepository.SignOutAsync();
        }
    }
}