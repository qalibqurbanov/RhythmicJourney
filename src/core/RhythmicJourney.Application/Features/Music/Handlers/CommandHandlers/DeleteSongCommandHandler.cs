using MediatR;
using System.Linq;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RhythmicJourney.Core.Entities.Music;
using RhythmicJourney.Application.Features.Music.Common;
using RhythmicJourney.Application.Features.Music.Commands;
using RhythmicJourney.Application.Contracts.Persistence.UnitOfWork.Abstractions;

namespace RhythmicJourney.Application.Features.Music.Handlers.CommandHandlers;

/// <summary>
/// Musiqinin silinmesi sorgusunu reallawdiran handler.
/// </summary>
public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, SongResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteSongCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        this._unitOfWork = unitOfWork;
        this._httpContextAccessor = httpContextAccessor;
    }

    public async Task<SongResult> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
    {
        int userID = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserID"));
        {
            Song song = _unitOfWork.SongRepository.GetSongs(song => (song.Id == request.DTO.SongID) && (song.UploaderID == userID)).FirstOrDefault();
            {
                if (song == null)
                {
                    return await SongResult.FailureAsync($"Song with ID {request.DTO.SongID} not found!");
                }
            }

            {
                _unitOfWork.SongRepository.Remove(song);
                int affectedRowCount = await _unitOfWork.SaveChangesToDB_StandartDb();

                return await SongResult.SuccessAsync($"Deleted {affectedRowCount} data.");
            }
        }
    }
}