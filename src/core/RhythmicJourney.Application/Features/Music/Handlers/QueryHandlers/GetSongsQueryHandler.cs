﻿using MediatR;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Queries;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Music.Handlers.QueryHandlers;

/// <summary>
/// Musiqilerin elde olunmasi sorgusunu reallawdiran handler.
/// </summary>
public class GetSongsQueryHandler : IRequestHandler<GetSongsQuery, SongResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetSongsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        this._unitOfWork = unitOfWork;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task<SongResult> Handle(GetSongsQuery request, CancellationToken cancellationToken)
    {
        int userID = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID"));
        {
            List<Song> songs = await _unitOfWork.SongRepository.GetSongs(song => song.UploaderID == userID).ToListAsync();
            {
                if (songs.Count == 0)
                {
                    return await SongResult.SuccessAsync("There is no songs.");
                }
                else
                {
                    return await SongResult.SuccessAsync(songs);
                }
            }
        }
    }
}