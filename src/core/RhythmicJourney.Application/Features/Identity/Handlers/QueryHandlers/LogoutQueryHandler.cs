﻿using MediatR;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Application.Repository.Abstract;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Features.Identity.Queries;

namespace RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

public class LogoutQueryHandler : IRequestHandler<LogoutQuery>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository
        )
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

        _refreshTokenRepository.RevokeUsersAllRefreshTokens(userID);
        await _userRepository.SignOut();
    }
}