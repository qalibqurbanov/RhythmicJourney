using MediatR;
using System.Linq;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions.Music;

namespace RhythmicJourney.Application.Features.Music.Handlers.CommandHandlers;

/// <summary>
/// Musiqinin silinmesi sorgusunu reallawdiran handler.
/// </summary>
public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, SongResult>
{
    private readonly ISongRepository _songRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteSongCommandHandler(ISongRepository songRepository, IHttpContextAccessor httpContextAccessor)
    {
        this._songRepository = songRepository;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task<SongResult> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        int userID = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID"));
        {
            Song song = _songRepository.GetSongs(song => (song.Id == request.DTO.SongID) && (song.UploaderID == userID)).FirstOrDefault();
            {
                if (song == null)
                {
                    return await SongResult.FailureAsync($"Song with ID {request.DTO.SongID} not found!");
                }
            }

            {
                int affectedRowCount = _songRepository.Remove(song);

                return await SongResult.SuccessAsync($"Deleted {affectedRowCount} data.");
            }
        }
    }
}